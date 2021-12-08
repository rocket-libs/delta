using delta.ProcessRunning;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delta.Running
{
    public interface IExternalProcessRunner
    {
        Task RunExternalProcessAsync(string command, string workingDirectory, Guid projectId);
    }

    public class ExternalProcessRunner : IExternalProcessRunner
    {
        private readonly IProcessRunner processRunner;
        private readonly IProcessRunnerLoggerBuilder processRunnerLogger;

        public ExternalProcessRunner(
            IProcessRunner processRunner,
            IProcessRunnerLoggerBuilder processRunnerLogger)
        {
            this.processRunner = processRunner;
            this.processRunnerLogger = processRunnerLogger;
        }

        public async Task RunExternalProcessAsync(string command, string workingDirectory, Guid projectId)
        {
            var commandParts = command.Trim().Split(new char[] { ' ' });
            var args = string.Empty;
            var app = commandParts[0];

            if (commandParts.Length > 1)
            {
                for (int i = 1; i < commandParts.Length; i++)
                {
                    args += $" {commandParts[i]}";
                }
            }
            var processStartInformation = new ProcessStartInformation
            {
                Filename = app,
                Arguments = args,
                WorkingDirectory = workingDirectory,
                Timeout = TimeSpan.FromMinutes(5)
            };
            var result = await processRunner.RunAsync(processStartInformation, projectId);
            result.RawCommand = command;
            await processRunnerLogger.LogAsync(result, projectId);
        }
    }
}