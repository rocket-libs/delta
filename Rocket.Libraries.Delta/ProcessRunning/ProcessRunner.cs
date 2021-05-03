using RunProcessAsTask;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace delta.ProcessRunning
{
    public interface IProcessRunner
    {
        Task<ProcessRunningResults> RunAsync(ProcessStartInformation processStartInformation);
    }

    public class ProcessRunner : IProcessRunner
    {
        public static SemaphoreSlim ProcessRunningSemaphore = new SemaphoreSlim(1, 1);

        public async Task<ProcessRunningResults> RunAsync(ProcessStartInformation processStartInformation)
        {
            try
            {
                await ProcessRunningSemaphore.WaitAsync();
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = processStartInformation.Filename,
                    Arguments = processStartInformation.Arguments,
                    WorkingDirectory = processStartInformation.WorkingDirectory
                };
                if (processStartInformation.Timeout == default)
                {
                    processStartInformation.Timeout = Timeout.InfiniteTimeSpan;
                }

                using (var cancellationTokenSource = new CancellationTokenSource(processStartInformation.Timeout))
                {
                    var result = await ProcessEx.RunAsync(processStartInfo, cancellationTokenSource.Token);
                    var processRunningResults = new ProcessRunningResults
                    {
                        Process = result.Process,
                        StandardError = result.StandardError,
                        ExitCode = result.ExitCode,
                        RunTime = result.RunTime,
                        StandardOutput = result.StandardOutput,
                    };
                    if (result.ExitCode != 0)
                    {
                        throw new ProcessRunningException(processStartInformation, processRunningResults);
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