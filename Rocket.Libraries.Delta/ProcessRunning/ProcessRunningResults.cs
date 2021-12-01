using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace delta.ProcessRunning
{
    public class ProcessRunningResults
    {
        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan Duration => EndTime - StartTime;

        public DateTime EndTime { get; set; }

        public string[] Errors { get; set; }

        public int ExitCode { get; set; }

        public string[] Output { get; set; }

        [JsonIgnore]
        public Process Process { get; set; }

        public string RawCommand { get; set; }

        public DateTime StartTime { get; set; }
    }
}