using System;

namespace Rocket.Libraries.Delta.GitInterfacing
{
    public class GitInterfaceCommand
    {
        public string Command { get; set; }
        public string WorkingDirectory { get; set; }

        public Guid ProjectId { get; set; }

        public string Branch { get; set; }

        public string Url { get; set; }
    }
}