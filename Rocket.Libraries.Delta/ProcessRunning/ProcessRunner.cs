using Rocket.Libraries.Delta.ExtensionsHelper;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
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
        Task<ProcessRunningResults> RunAsync(ProcessStartInformation processStartInformation);
    }

    public class ProcessRunner : IProcessRunner
    {
        public static SemaphoreSlim ProcessRunningSemaphore = new SemaphoreSlim(1, 1);
        private readonly IExtensionHelper extensionHelper;
        private readonly IProcessRunnerLoggerBuilder processRunnerLoggerBuilder;

        public ProcessRunner(
            IExtensionHelper extensionHelper,
            IProcessRunnerLoggerBuilder processRunnerLoggerBuilder)
        {
            this.processRunnerLoggerBuilder = processRunnerLoggerBuilder;
            this.extensionHelper = extensionHelper;
        }

        public async Task<ProcessRunningResults> RunAsync(ProcessStartInformation processStartInformation)
        {
            try
            {
                await ProcessRunningSemaphore.WaitAsync();
                var effectiveFilename = ResolveFilename(processStartInformation.Filename);
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
                        processRunnerLoggerBuilder.Log(processRunningResults);
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

        private string ResolveFilename(string filename)
        {
            
            var extensionFilename = extensionHelper.GetExtensionExecutablePath(filename);
            if(File.Exists(extensionFilename))
            {
                return extensionFilename;
            }
            else
            {
                return filename;
            }
        }

        
    }
}