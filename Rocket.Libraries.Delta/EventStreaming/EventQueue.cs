using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rocket.Libraries.Delta.ProcessRunnerLogging;

namespace Rocket.Libraries.Delta.EventStreaming
{
    public interface IEventQueue : IEventListener
    {
        Task ListenAsync (string listenerId);

    }

    public class EventQueue : IEventQueue
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEventStreamer eventStreamer;
        private readonly IProcessRunnerLoggerBuilder processRunnerLoggerBuilder;

        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim (1, 1);

        private Queue<string> messages = new Queue<string> ();

        
        public EventQueue (
            IHttpContextAccessor httpContextAccessor,
            IEventStreamer eventStreamer
        )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.eventStreamer = eventStreamer;
        }

        public async Task ListenAsync (string listenerId)
        {
            try
            {
                await semaphoreSlim.WaitAsync ();
                eventStreamer.AddListener (listenerId, this);
                if (messages.Count > 0)
                {
                    await SendMessageAsync (messages.Dequeue ());
                }
                else
                {
                    await SendMessageAsync (string.Empty);
                }
            }
            finally
            {
                semaphoreSlim.Release ();
            }
        }

        public async Task OnEventAsync (string text)
        {
            try
            {
                await semaphoreSlim.WaitAsync ();
                if (!string.IsNullOrEmpty (text))
                {
                    messages.Enqueue (text);
                }
            }
            finally
            {
                semaphoreSlim.Release ();
            }
        }

        private async Task SendMessageAsync (string text)
        {
            httpContextAccessor.HttpContext.Response.Headers.Add ("Content-Type", "text/event-stream");
            if (!string.IsNullOrEmpty (text))
            {
                text = $"data: {text}\n\n";
                var messageBytes = System.Text.Encoding.ASCII.GetBytes (text);
                await httpContextAccessor.HttpContext.Response.Body.WriteAsync (messageBytes, 0, messageBytes.Length);
            }
            await httpContextAccessor.HttpContext.Response.Body.FlushAsync ();
        }

    }
}