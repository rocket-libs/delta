using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rocket.Libraries.Delta.ProcessRunnerLogging;

namespace Rocket.Libraries.Delta.EventStreaming
{
    public interface IEventQueue
    {
        Task CloseAsync(object queueId);
        Task DequeueAsync(object queueId);
        Task EnqueueManyAsync(object queueId, IEnumerable<string> messages);

        Task EnqueueSingleAsync(object queueId, string message);
    }

    public class EventQueue : IEventQueue
    {
        private string terminateMessage = "---terminate---";

        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private Dictionary<string, Queue<string>> messageQueues = new Dictionary<string, Queue<string>>();
        private readonly IHttpContextAccessor httpContextAccessor;

        public EventQueue(
            IHttpContextAccessor httpContextAccessor
        )
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task CloseAsync(object queueId)
        {
            var queueIdString = getQueueIdString(queueId);
            if (messageQueues.ContainsKey(queueIdString))
            {
                await EnqueueManyAsync(queueIdString, new List<string>{ terminateMessage});
            }
        }

        public async Task EnqueueManyAsync(object queueId, IEnumerable<string> messages)
        {
            try
            {
                var queueIdString = getQueueIdString(queueId);
                await semaphoreSlim.WaitAsync();
                if (!messageQueues.ContainsKey(queueIdString))
                {
                    messageQueues.Add(queueIdString, new Queue<string>());
                }
                var combinedMessage = string.Join("<br/>", messages);
                messageQueues[queueIdString].Enqueue(combinedMessage);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task DequeueAsync(object queueId)
        {
            try
            {
                var queueIdString = getQueueIdString(queueId);
                await semaphoreSlim.WaitAsync();
                if (messageQueues.ContainsKey(queueIdString))
                {
                    if (messageQueues[queueIdString].Count > 0)
                    {
                        var message = messageQueues[queueIdString].Dequeue();
                        var isTerminate = message == terminateMessage;
                        if (isTerminate)
                        {
                            messageQueues.Remove(queueIdString);
                        }
                        await TransmitAsync(message);
                    }
                    else
                    {
                        await TransmitAsync(string.Empty);
                    }
                }
                else
                {
                    await TransmitAsync(string.Empty);
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task TransmitAsync(string text)
        {
            httpContextAccessor.HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            if (!string.IsNullOrEmpty(text))
            {
                text = $"data: {text}\n\n";
                var messageBytes = System.Text.Encoding.ASCII.GetBytes(text);
                await httpContextAccessor.HttpContext.Response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
            await httpContextAccessor.HttpContext.Response.Body.FlushAsync();
        }

        private string getQueueIdString(object queueId)
        {
            if (queueId == default)
            {
                throw new ArgumentNullException($"{nameof(queueId)} cannot be null");
            }
            return queueId.ToString().ToLower();
        }

        public async Task EnqueueSingleAsync(object queueId, string message)
        {
            await EnqueueManyAsync(queueId, new List<string>{ message});

        }
    }
}
