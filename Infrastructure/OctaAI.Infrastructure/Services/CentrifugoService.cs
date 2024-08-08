using Centrifugo.AspNetCore.Abstractions;
using Centrifugo.AspNetCore.Extensions;
using OctaAI.Application.Interfaces;

namespace OctaAI.Infrastructure.Services
{
    public class CentrifugoService : ICentrifugoService
    {
        private readonly ICentrifugoClient _centrifugoClient;

        public CentrifugoService(ICentrifugoClient centrifugoClient)
        {
            _centrifugoClient = centrifugoClient;
        }

        public async Task Broadcast(object data, params string[] channels)
        {
            await _centrifugoClient.Broadcast(new { value = data }, channels);
        }

        public async Task<string> GetChannelList()
        {
            var channelsInfo = await _centrifugoClient.Channels();
            return channelsInfo.ToString();
        }

        public async Task Publish(object data, string channel)
        {
            await _centrifugoClient.Publish(new { value = data }, channel);
        }

        public async Task Subscribe(string user, string channel)
        {
            await _centrifugoClient.Subscribe(user, channel);
        }

        public async Task UnSubscribe(string user, string channel)
        {
            await _centrifugoClient.UnSubscribe(user, channel);
        }
    }
}
