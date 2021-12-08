using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using delta.ProcessRunning;
using Rocket.Libraries.Delta.EventStreaming;

namespace Rocket.Libraries.Delta.ProcessRunnerLogging
{
    public interface IProcessRunnerLoggerBuilder
    {
        ImmutableList<ProcessRunningResults> Peek { get; }

        ImmutableList<ProcessRunningResults> Build();
        Task<IProcessRunnerLoggerBuilder> LogAsync(ProcessRunningResults processRunningResults, Guid eventId);
        Task<IProcessRunnerLoggerBuilder> LogToError(string error, Guid eventId);
        Task<IProcessRunnerLoggerBuilder> LogToOutputAsync(string message, Guid eventId);
    }

    public class ProcessRunnerLoggerBuilder : IProcessRunnerLoggerBuilder
    {
        private List<ProcessRunningResults> processRunningResults = new List<ProcessRunningResults>();
        private readonly IEventStreamer eventStreamer;

        public ProcessRunnerLoggerBuilder(
            IEventStreamer eventStreamer
        )
        {
            this.eventStreamer = eventStreamer;
        }

        public async Task<IProcessRunnerLoggerBuilder> LogToOutputAsync(string message, Guid eventId)
        {
            return await LogAsync(new ProcessRunningResults
            {
                Output = new List<string> { message }.ToArray()
            }, eventId);
        }

        public async Task<IProcessRunnerLoggerBuilder> LogToError(string error, Guid eventId)
        {
            return await LogAsync(new ProcessRunningResults
            {
                Errors = new List<string> { error }.ToArray()
            }, eventId);
        }

        public async Task<IProcessRunnerLoggerBuilder> LogAsync(ProcessRunningResults processRunningResults, Guid eventId)
        {
            var list = processRunningResults.Output;
            if (list == null || list.Length == 0)
            {
                list = processRunningResults.Errors;
            }
            if (list != null)
            {
                foreach (var item in list)
                {
                    await eventStreamer.StreamMessageAsync(eventId.ToString(), item);
                }
            }
            this.processRunningResults.Add(processRunningResults);
            return this;
        }

        public ImmutableList<ProcessRunningResults> Peek
        {
            get
            {
                return processRunningResults.ToImmutableList();
            }
        }

        public ImmutableList<ProcessRunningResults> Build()
        {
            var result = Peek;
            processRunningResults = new List<ProcessRunningResults>();
            return result;
        }
    }
}