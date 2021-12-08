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
        Task<IProcessRunnerLoggerBuilder> LogAsync(ProcessRunningResults processRunningResults, Guid projectId);
        Task<IProcessRunnerLoggerBuilder> LogToError(string error, Guid projectId);
        Task<IProcessRunnerLoggerBuilder> LogToOutputAsync(string message, Guid projectId);
    }

    public class ProcessRunnerLoggerBuilder : IProcessRunnerLoggerBuilder
    {
        private List<ProcessRunningResults> processRunningResults = new List<ProcessRunningResults>();
        private readonly IEventQueue eventStreamer;

        public ProcessRunnerLoggerBuilder(
            IEventQueue eventStreamer
        )
        {
            this.eventStreamer = eventStreamer;
        }

        public async Task<IProcessRunnerLoggerBuilder> LogToOutputAsync(string message, Guid projectId)
        {
            return await LogAsync(new ProcessRunningResults
            {
                Output = new List<string> { message }.ToArray()
            }, projectId);
        }

        public async Task<IProcessRunnerLoggerBuilder> LogToError(string error, Guid projectId)
        {
            return await LogAsync(new ProcessRunningResults
            {
                Errors = new List<string> { error }.ToArray()
            }, projectId);
        }

        public async Task<IProcessRunnerLoggerBuilder> LogAsync(ProcessRunningResults processRunningResults, Guid projectId)
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
                    await eventStreamer.EnqueueAsync(projectId, item);
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