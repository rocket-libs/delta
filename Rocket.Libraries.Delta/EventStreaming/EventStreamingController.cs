using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocket.Libraries.CallProxying.Services;
using Rocket.Libraries.Delta.Configuration.Routing;

namespace Rocket.Libraries.Delta.EventStreaming
{
    public class EventStreamingController : DeltaController
    {
        private readonly IEventStreamer eventStreamer;

        public EventStreamingController(
            ICallProxy callProxy,
            IEventStreamer eventStreamer)
             : base(callProxy)
        {
            this.eventStreamer = eventStreamer;
        }


        [HttpGet("listen")]
        public async Task ListenAsync()
        {
            await eventStreamer.ListenAsync();
        }
    }
}