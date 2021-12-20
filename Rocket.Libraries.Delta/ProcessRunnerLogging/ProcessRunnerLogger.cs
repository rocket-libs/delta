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
        ImmutableList<ProcessRunningResults> PeekAll { get; }
        ImmutableList<string> PeekErrors { get; }

        ImmutableList<ProcessRunningResults> Build();
        Task<IProcessRunnerLoggerBuilder> LogAsync(ProcessRunningResults processRunningResults, Guid projectId);
        Task<IProcessRunnerLoggerBuilder> LogToError(string error, Guid projectId);
        Task<IProcessRunnerLoggerBuilder> LogToOutputAsync(string message, Guid projectId);
    }

    public class ProcessRunnerLoggerBuilder : IProcessRunnerLoggerBuilder
    {
        private List<ProcessRunningResults> processRunningResults = new List<ProcessRunningResults>();

        private List<string> errors = new List<string>();
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
            var messages = processRunningResults.Output;
            if (messages == null || messages.Length == 0)
            {
                messages = processRunningResults.Errors;
                errors.AddRange(processRunningResults.Errors);
            }
            await eventStreamer.EnqueueManyAsync(projectId, messages);
            this.processRunningResults.Add(processRunningResults);
            return this;
        }

        public ImmutableList<ProcessRunningResults> PeekAll
        {
            get
            {
                return processRunningResults.ToImmutableList();
            }
        }

        public ImmutableList<string> PeekErrors
        {
            get
            {
                return errors.ToImmutableList();
            }
        }

        public ImmutableList<ProcessRunningResults> Build()
        {
            var result = PeekAll;
            processRunningResults = new List<ProcessRunningResults>();
            return result;
        }
    }
}