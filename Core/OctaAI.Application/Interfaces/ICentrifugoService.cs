using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctaAI.Application.Interfaces
{
    public interface ICentrifugoService
    {
        public Task<string> GetChannelList();
        public Task Subscribe(string user, string channel);
        public Task UnSubscribe(string user, string channel);
        public Task Publish(object data, string channel);
        public Task Broadcast(object data, params string[] channels);
    }
}
