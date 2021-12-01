using delta.ProcessRunning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delta.Running
{
    public interface IExternalProcessRunner
    {
        Task<ProcessRunningResults> RunExternalProcessAsync(string command, string workingDirectory);
    }

    public class ExternalProcessRunner : IExternalProcessRunner
    {
        private readonly IProcessRunner processRunner;

        public ExternalProcessRunner(
            IProcessRunner processRunner)
        {
            this.processRunner = processRunner;
        }

        public async Task<ProcessRunningResults> RunExternalProcessAsync(string command, string workingDirectory)
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
            var result = await processRunner.RunAsync(processStartInformation);
            result.RawCommand = command;
            return result;
        }
    }
}