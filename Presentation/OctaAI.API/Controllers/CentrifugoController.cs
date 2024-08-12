using Centrifugo.AspNetCore.Abstractions;
using Centrifugo.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OctaAI.Application.Dtos.Centrifugo;
using OctaAI.Application.Interfaces;
using OctaAI.Domain.Entities;
using OctaAI.Persistence.Contexts.Application;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctaAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrifugoController : ControllerBase
    {
        private readonly ICentrifugoService _centrifugoService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IIdentityService _identityService;

        public CentrifugoController(ICentrifugoService centrifugoService, ApplicationDbContext applicationDbContext, IIdentityService identityService)
        {
            _centrifugoService = centrifugoService;
            _applicationDbContext = applicationDbContext;
            _identityService = identityService;
        }

        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe([FromForm] CentrifugoSubscribeRequest centrifugoSubscribeRequest)
        {
            //await _centrifugoService.Subscribe(centrifugoSubscribeRequest.UserId.ToString(), centrifugoSubscribeRequest.Channel);

            //var userWithChannels = _applicationDbContext.Users
            //    .Where(u => u.Id == Guid.Parse(centrifugoSubscribeRequest.UserId))
            //    .Include(u => u.Channels)
            //    .FirstOrDefault();

            //var channelId = Guid.Parse(centrifugoSubscribeRequest.Channel);

            //var userChannel = new UserChannel
            //{
            //    ChannelId = channelId,
            //    Id = channelId,
            //    UserId = Guid.Parse(centrifugoSubscribeRequest.UserId),
            //    CreatedByUserId = Guid.Parse(centrifugoSubscribeRequest.UserId),
            //    //User = User
            //};
            
            //userWithChannels.Channels.Add(userChannel);

            //_applicationDbContext.Add(userChannel);
            //_applicationDbContext.Update(userWithChannels);

            //_applicationDbContext.SaveChanges();

            return Ok();
        }

        // GET: api/<CentrifugoController>
        [HttpGet("Test")]
        public async Task<IActionResult> Centrifugo()
        {
            //await _centrifugoClient.Subscribe("kado2", "channel2");

            // Also you can use simple versions of methods
            await _centrifugoService.Publish(new { value = 123 }, "channel");
            //await _centrifugoClient.Broadcast(new { value = 1 }, "channel", "channel2");

            // Get server info
            //var serverInfo = await _centrifugoService.Info();

            // Get channels info
            //var channelsInfo = await _centrifugoService.Channels();

            //return Ok(new { serverInfo, channelsInfo });
            return Ok();
        }

        [HttpGet("GetChannel")]
        public async Task<IActionResult> GetChannelInfo()
        {
            //var channelsInfo = await _centrifugoService.Channels();

            return Ok();
        }



    }
}
