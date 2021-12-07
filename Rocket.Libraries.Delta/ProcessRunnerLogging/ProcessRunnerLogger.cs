using System.Collections.Generic;
using System.Collections.Immutable;
using delta.ProcessRunning;
using Rocket.Libraries.Delta.EventStreaming;

namespace Rocket.Libraries.Delta.ProcessRunnerLogging
{
    public interface IProcessRunnerLoggerBuilder
    {
        ImmutableList<ProcessRunningResults> Peek { get; }

        ImmutableList<ProcessRunningResults> Build ();
        IProcessRunnerLoggerBuilder Log (ProcessRunningResults processRunningResults);
        IProcessRunnerLoggerBuilder LogToError (string error);
        IProcessRunnerLoggerBuilder LogToOutput (string message);
    }

    public class ProcessRunnerLoggerBuilder : IProcessRunnerLoggerBuilder
    {
        private List<ProcessRunningResults> processRunningResults = new List<ProcessRunningResults> ();
        private readonly IEventStreamer eventStreamer;

        public ProcessRunnerLoggerBuilder (
            IEventStreamer eventStreamer
        )
        {
            this.eventStreamer = eventStreamer;
        }

        public IProcessRunnerLoggerBuilder LogToOutput (string message)
        {
            eventStreamer.StreamDataAsync (message);
            return Log (new ProcessRunningResults
            {
                Output = new List<string> { message }.ToArray ()
            });
        }

        public IProcessRunnerLoggerBuilder LogToError (string error)
        {
            return Log (new ProcessRunningResults
            {
                Errors = new List<string> { error }.ToArray ()
            });
        }

        public IProcessRunnerLoggerBuilder Log (ProcessRunningResults processRunningResults)
        {
            this.processRunningResults.Add (processRunningResults);
            return this;
        }

        public ImmutableList<ProcessRunningResults> Peek
        {
            get
            {
                return processRunningResults.ToImmutableList ();
            }
        }

        public ImmutableList<ProcessRunningResults> Build ()
        {
            var result = Peek;
            processRunningResults = new List<ProcessRunningResults> ();
            return result;
        }
    }
}