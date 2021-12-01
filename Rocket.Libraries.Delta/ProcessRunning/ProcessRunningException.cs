using System;
using System.Linq;

namespace delta.ProcessRunning
{
    public class ProcessRunningException : Exception
    {
        public ProcessRunningException(ProcessStartInformation processStartInformation, ProcessRunningResults processRunningResults)
            : base(
                  message: $"Exit Code: {processRunningResults.ExitCode} from {processStartInformation.Filename} {processStartInformation.Arguments}\nCheck to see if inner exception has more information.",
                  innerException: processRunningResults.Errors != null && processRunningResults.Errors.Length > 0 ? new Exception(processRunningResults.Errors.First()) : null)
        {
            ProcessStartInformation = processStartInformation;
            ProcessRunningResults = processRunningResults;
        }

        public ProcessRunningResults ProcessRunningResults { get; }

        public ProcessStartInformation ProcessStartInformation { get; }
    }
}