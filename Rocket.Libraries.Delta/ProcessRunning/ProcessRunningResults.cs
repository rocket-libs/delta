using System;
using System.Diagnostics;

namespace delta.ProcessRunning
{
    public class ProcessRunningResults
    {
        public int ExitCode { get; set; }

        public Process Process { get; set; }

        public TimeSpan RunTime { get; set; }

        public string[] StandardError { get; set; }

        public string[] StandardOutput { get; set; }
    }
}