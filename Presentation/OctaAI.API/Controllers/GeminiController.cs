﻿using Centrifugo.AspNetCore.Abstractions;
using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OctaAI.Application.Dtos;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctaAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly IGeminiClient _geminiClient;


        public GeminiController(IGeminiClient geminiClient)
        {
            _geminiClient = geminiClient;
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

            var responseString = response.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text.ToString();

            var res = JsonSerializer.Serialize(responseString);

            return Ok(res);
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
