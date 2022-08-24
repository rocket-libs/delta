using Rocket.Libraries.Delta.EventStreaming;
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
        Task<ProcessRunningResults> RunAsync(
            ProcessStartInformation processStartInformation,
            Guid projectId,
            Func<ProcessResults, bool> customSuccessEvaluator = null);
    }

    public class ProcessRunner : IProcessRunner
    {
        public static SemaphoreSlim ProcessRunningSemaphore = new SemaphoreSlim(1, 1);

        private readonly IEventQueue eventQueue;

        private readonly IProcessFilenameResolver processFilenameResolver;

        private readonly IProcessRunnerLoggerBuilder processRunnerLoggerBuilder;

        public ProcessRunner(
            IProcessRunnerLoggerBuilder processRunnerLoggerBuilder,
            IProcessFilenameResolver processFilenameResolver,
            IEventQueue eventQueue)
        {
            this.processRunnerLoggerBuilder = processRunnerLoggerBuilder;
            this.processFilenameResolver = processFilenameResolver;
            this.eventQueue = eventQueue;
        }

        public async Task<ProcessRunningResults> RunAsync(
            ProcessStartInformation processStartInformation,
            Guid projectId,
            Func<ProcessResults, bool> customSuccessEvaluator = null)
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
                    await processRunnerLoggerBuilder.LogToOutputAsync($"Running in {processStartInformation.WorkingDirectory}",projectId);
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
                    var succeeded = customSuccessEvaluator != null ? customSuccessEvaluator(result) : result.ExitCode == 0;
                    if (succeeded == false)
                    {
                        await processRunnerLoggerBuilder.LogAsync(processRunningResults, projectId);
                        if (processRunningResults.Errors != null && processRunningResults.Errors.Length > 0)
                        {
                            await eventQueue.EnqueueManyAsync(projectId, processRunningResults.Errors);
                        }
                        throw new Exception($"Exiting due to non-zero exit code when running process '{effectiveFilename} {processStartInformation.Arguments}'");
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
            catch (Exception ex)
            {
                throw new Exception($"Error running process '{processStartInformation.Filename} {processStartInformation.Arguments}'", ex);
            }
            finally
            {
                ProcessRunningSemaphore.Release();
            }
        }
    }
}