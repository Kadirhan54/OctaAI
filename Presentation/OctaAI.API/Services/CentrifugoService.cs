using Centrifugo.AspNetCore.Abstractions;
using Centrifugo.AspNetCore.Extensions;

namespace OctaAI.API.Services
{
    public class CentrifugoService
    {
        private readonly ICentrifugoClient _centrifugoClient;

        public CentrifugoService(ICentrifugoClient centrifugoClient)
        {
            _centrifugoClient = centrifugoClient;
        }

        public async Task TestCentrifugo()
        {


            // Broadcast something
            await _centrifugoClient.Broadcast(
                new Centrifugo.AspNetCore.Models.Request.BroadcastParams()
                {
                    Channels = new[] { "channel", "channel2" },
                    Data = new { value = 1 }
                });

            // Publish something
            await _centrifugoClient.Publish(
                new Centrifugo.AspNetCore.Models.Request.PublishParams()
                {
                    Channel = "channel",
                    Data = new { value = 1 }
                });

            // Also you can use simple versions of methods
            await _centrifugoClient.Publish(new { value = 1 }, "channel");
            await _centrifugoClient.Broadcast(new { value = 1 }, "channel", "channel2");

            // Get server info
            var serverInfo = await _centrifugoClient.Info();

            // Get channels info
            var channelsInfo = await _centrifugoClient.Channels();
        }
    }
}
