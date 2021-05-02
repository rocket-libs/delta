using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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