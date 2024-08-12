using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OctaAI.Application.Dtos.Gemini;
using OctaAI.Application.Interfaces;
using OctaAI.Domain.Entities;
using OctaAI.Persistence.Contexts.Application;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctaAI.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly IGeminiClient _geminiClient;
        private readonly ICentrifugoService _centrifugoService;
        private readonly ApplicationDbContext _applicationDbContext;


        public GeminiController(IGeminiClient geminiClient, ICentrifugoService centrifugoService, ApplicationDbContext applicationDbContext)
        {
            _geminiClient = geminiClient;
            _centrifugoService= centrifugoService;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _geminiClient.GetModels();

            return Ok(response);
        }

        [HttpPost("PromptText")]
        public async Task<IActionResult> PostTextAsync([FromForm] TextPromptRequestDto textPromptRequestDto)
        {
            var response = await _geminiClient.TextPrompt(textPromptRequestDto.Value);

            if (response?.Candidates == null || response.Candidates.Count == 0)
            {
                return BadRequest("No candidates found");
            }

            // user already subscribes to the channel in the frontend so this is not needed
            //await _centrifugoService.Subscribe("user", textPromptRequestDto.Channel);

            var responseString = response.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text.ToString();

            var res = JsonSerializer.Serialize(responseString);

            await _centrifugoService.Subscribe(textPromptRequestDto.UserId.ToString(), textPromptRequestDto.Channel);

            await _centrifugoService.Publish(res, textPromptRequestDto.Channel);

            var userWithChannels = _applicationDbContext.Users
                .Where(u => u.Id == Guid.Parse(textPromptRequestDto.UserId))
                .Include(u => u.Channels)
                .FirstOrDefault();

            var channelId = Guid.Parse(textPromptRequestDto.Channel);

            var userChannel = new UserChannel
            {
                ChannelId = channelId,
                Id = channelId,
                UserId = Guid.Parse(textPromptRequestDto.UserId),
                CreatedByUserId = Guid.Parse(textPromptRequestDto.UserId),
                //User = User
            };

            AiResponse aiResponse = new AiResponse
            {
                Channel = textPromptRequestDto.Channel,
                Message = responseString,
                UserId = Guid.Parse(textPromptRequestDto.UserId)
            };

            userWithChannels.Channels.Add(userChannel);

            _applicationDbContext.Add(aiResponse);
            _applicationDbContext.Add(userChannel);
            _applicationDbContext.Update(userWithChannels);

            _applicationDbContext.SaveChanges();

            return Ok();
        }   

        // TODO : Convert this to a ImagePromtRequestDto
        [HttpPost("PromptImage")]
        public async Task<IActionResult> PostImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is not selected or is empty");
            }

            byte[] image;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }

            var response = await _geminiClient.ImagePrompt("Describe this image", image, ImageMimeType.Jpeg);

            if (response?.Candidates == null || response.Candidates.Count == 0)
            {
                return BadRequest("No candidates found");
            }

            return Ok(response.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text.ToString());

        }
    }
}
