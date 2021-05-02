using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delta.ProcessRunning
{
    public class ProcessStartInformation
    {
        public string Arguments { get; set; }

        public string Filename { get; set; }

        public TimeSpan Timeout { get; set; }

        public string WorkingDirectory { get; set; }
    }
}