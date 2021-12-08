using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.ProcessRunnerLogging;

namespace Rocket.Libraries.Delta.EventStreaming
{
    public interface IEventStreamer
    {
        void AddListener (string listenerId, IEventListener listener);
        Task RemoveListenerAsync (string listenerId);
        Task StreamMessageAsync (string listenerId, string message);
    }

    public class EventStreamer : IEventStreamer
    {
        private string terminateMessage = "---terminate---";

        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim (1, 1);
        private Dictionary<string, IEventListener> eventListeners = new Dictionary<string, IEventListener> ();

        public void AddListener (string listenerId, IEventListener listener)
        {
            listenerId = listenerId.ToLower ();
            if (!eventListeners.ContainsKey (listenerId))
            {
                eventListeners.Add (listenerId, listener);
            }
        }

        public async Task RemoveListenerAsync (string listenerId)
        {
            listenerId = listenerId.ToLower ();
            if (eventListeners.ContainsKey (listenerId))
            {
                await StreamMessageAsync (listenerId, terminateMessage);
                eventListeners.Remove (listenerId);
            }
        }

        public async Task StreamMessageAsync (string listenerId, string message)
        {
            try
            {
                listenerId = listenerId.ToLower ();
                await semaphoreSlim.WaitAsync ();
                if (eventListeners.ContainsKey (listenerId))
                {
                    await eventListeners[listenerId].OnEventAsync (message);
                }
            }
            finally
            {
                semaphoreSlim.Release ();
            }
        }
    }
}