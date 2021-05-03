using System;

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