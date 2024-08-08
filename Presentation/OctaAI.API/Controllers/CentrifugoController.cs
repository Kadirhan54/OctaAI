using Centrifugo.AspNetCore.Abstractions;
using Centrifugo.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctaAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrifugoController : ControllerBase
    {
        private readonly ICentrifugoClient _centrifugoClient;

        public CentrifugoController(ICentrifugoClient centrifugoClient)
        {
            _centrifugoClient = centrifugoClient;
        }

        // GET: api/<CentrifugoController>
        [HttpGet("Test")]
        public async Task<IActionResult> Centrifugo()
        {
            //await _centrifugoClient.Subscribe("kado2", "channel2");

            // Also you can use simple versions of methods
            await _centrifugoClient.Publish(new { value = 123 }, "channel");
            //await _centrifugoClient.Broadcast(new { value = 1 }, "channel", "channel2");

            // Get server info
            var serverInfo = await _centrifugoClient.Info();

            // Get channels info
            var channelsInfo = await _centrifugoClient.Channels();

            return Ok(new { serverInfo, channelsInfo });
        }

        [HttpGet("GetChannel")]
        public async Task<IActionResult> GetChannelInfo()
        {
            var channelsInfo = await _centrifugoClient.Channels();

            return Ok(channelsInfo);
        }

    }
}
