using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rocket.Libraries.Delta.EventStreaming
{
    public interface IEventStreamer
    {
        Task ListenAsync ();
        Task StreamDataAsync (string text);
    }

    public class EventStreamer : IEventStreamer
    {
        private bool open = false;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EventStreamer (
            IHttpContextAccessor httpContextAccessor
        )
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task ListenAsync ()
        {
            if (open)
            {
                await SendDataAsync ("Already listening");
                return;
            }
            open = true;
            await SendDataAsync ("Started");
        }

        public async Task StreamDataAsync (string text)
        {
            await SendDataAsync (text);
        }

        private async Task SendDataAsync (string text)
        {
            text = $"data: {text}\n\n";
            var messageBytes = System.Text.Encoding.ASCII.GetBytes (text);
            httpContextAccessor.HttpContext.Response.Headers.Add ("Content-Type", "text/event-stream");
            await httpContextAccessor.HttpContext.Response.Body.WriteAsync (messageBytes, 0, messageBytes.Length);
            await httpContextAccessor.HttpContext.Response.Body.FlushAsync ();
        }
    }
}