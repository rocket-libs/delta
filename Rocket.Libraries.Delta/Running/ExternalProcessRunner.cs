﻿using delta.ProcessRunning;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
using Rocket.Libraries.Delta.Projects;
using Rocket.Libraries.Delta.Variables;
using RunProcessAsTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delta.Running
{
    public interface IExternalProcessRunner
    {
        Task<ProcessRunningResults> RunExternalProcessAsync(
            string command,
            string workingDirectory,
            Guid projectId,
            Func<ProcessResults, bool> customSuccessEvaluator = null);

        Task<ProcessRunningResults> RunExternalProcessAsync(
            BuildCommand buildCommand,
            string workingDirectory,
            Guid projectId,
            Func<ProcessResults, bool> customSuccessEvaluator = null);
    }

    public class ExternalProcessRunner : IExternalProcessRunner
    {
        private readonly IProcessRunner processRunner;

        private readonly IProcessRunnerLoggerBuilder processRunnerLogger;

        private readonly IVariableManager variableManager;

        public ExternalProcessRunner(
            IProcessRunner processRunner,
            IProcessRunnerLoggerBuilder processRunnerLogger,
            IVariableManager variableManager)
        {
            this.processRunner = processRunner;
            this.processRunnerLogger = processRunnerLogger;
            this.variableManager = variableManager;
        }

        public async Task<ProcessRunningResults> RunExternalProcessAsync(
            BuildCommand buildCommand,
            string workingDirectory,
            Guid projectId,
            Func<ProcessResults, bool> customSuccessEvaluator = null)
        {
            var parsedCommand = variableManager.GetCommandParsedVariable(projectId, buildCommand.Command.Trim());
            var commandParts = parsedCommand.Trim().Split(new char[] { ' ' });
            var args = string.Empty;
            var app = commandParts[0];
            if (variableManager.IsVariableSetRequest(app))
            {
                variableManager.SetVariable(projectId, commandParts[1], processRunnerLogger.PeekAll.Last().Output.FirstOrDefault());
                return new ProcessRunningResults
                {
                    ExitCode = 0,
                };
            }
            else
            {
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
                    Timeout = TimeSpan.FromMinutes(40)
                };
                var result = await processRunner.RunAsync(processStartInformation, projectId, customSuccessEvaluator);
                result.RawCommand = buildCommand.Command;
                await processRunnerLogger.LogAsync(result, projectId);
                return result;
            }
        }

        public async Task<ProcessRunningResults> RunExternalProcessAsync(
            string command,
            string workingDirectory,
            Guid projectId,
            Func<ProcessResults, bool> customSuccessEvaluator = null)
        {
            return await RunExternalProcessAsync(
                new BuildCommand { Command = command },
                workingDirectory,
                projectId,
                customSuccessEvaluator);
        }
    }
}