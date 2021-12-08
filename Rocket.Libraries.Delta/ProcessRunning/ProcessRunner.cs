using Rocket.Libraries.Delta.ExtensionsHelper;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
using Rocket.Libraries.Delta.ProcessRunning;
using RunProcessAsTask;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace delta.ProcessRunning
{
    public interface IProcessRunner
    {
        Task<ProcessRunningResults> RunAsync(ProcessStartInformation processStartInformation, Guid projectId);
    }

    public class ProcessRunner : IProcessRunner
    {
        public static SemaphoreSlim ProcessRunningSemaphore = new SemaphoreSlim(1, 1);

        private readonly IProcessRunnerLoggerBuilder processRunnerLoggerBuilder;
        private readonly IProcessFilenameResolver processFilenameResolver;

        public ProcessRunner(
            IProcessRunnerLoggerBuilder processRunnerLoggerBuilder,
            IProcessFilenameResolver processFilenameResolver)
        {
            this.processRunnerLoggerBuilder = processRunnerLoggerBuilder;
            this.processFilenameResolver = processFilenameResolver;
        }

        public async Task<ProcessRunningResults> RunAsync(ProcessStartInformation processStartInformation, Guid projectId)
        {
            try
            {
                await ProcessRunningSemaphore.WaitAsync();
                var effectiveFilename = processFilenameResolver.ResolveFilename(processStartInformation.Filename);
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = effectiveFilename,
                    Arguments = processStartInformation.Arguments,
                    WorkingDirectory = processStartInformation.WorkingDirectory,
                };
                if (processStartInformation.Timeout == default)
                {
                    processStartInformation.Timeout = Timeout.InfiniteTimeSpan;
                }

                using (var cancellationTokenSource = new CancellationTokenSource(processStartInformation.Timeout))
                {
                    var startTime = DateTime.Now;
                    await processRunnerLoggerBuilder.LogToOutputAsync($"Starting process {effectiveFilename} {processStartInformation.Arguments}", projectId);
                    var result = await ProcessEx.RunAsync(processStartInfo, cancellationTokenSource.Token);
                    var endTime = DateTime.Now;
                    var processRunningResults = new ProcessRunningResults
                    {
                        Process = result.Process,
                        Errors = result.StandardError,
                        ExitCode = result.ExitCode,
                        StartTime = startTime,
                        EndTime = endTime,
                        Output = result.StandardOutput,
                    };
                    if (result.ExitCode != 0)
                    {
                        await processRunnerLoggerBuilder.LogAsync(processRunningResults, projectId);
                        throw new Exception("Exiting due to non-zero exit code");
                    }
                    else
                    {
                        return processRunningResults;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw new Exception($"Timeout of {processStartInformation.Timeout} hit while trying to run {processStartInformation.Filename} {processStartInformation.Arguments}");
            }
            finally
            {
                ProcessRunningSemaphore.Release();
            }
        }




    }
}