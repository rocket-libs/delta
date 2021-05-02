using RunProcessAsTask;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                    if (result.ExitCode != 0)
                    {
                        throw new Exception(
                            $"Exit Code: {result.ExitCode} from {processStartInformation.Filename} {processStartInformation.Arguments}",
                            innerException: result.StandardError != null && result.StandardError.Length > 0 ? new Exception(result.StandardError.First()) : null);
                    }
                    return new ProcessRunningResults
                    {
                        Process = result.Process,
                        StandardError = result.StandardError,
                        ExitCode = result.ExitCode,
                        RunTime = result.RunTime,
                        StandardOutput = result.StandardOutput,
                    };
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