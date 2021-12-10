using System;
using System.IO;
using System.Reflection;

namespace Rocket.Libraries.Delta.ProcessRunning
{
    public interface IWorkingDirectoryRootProvider
    {
        string WorkingDirectoryRoot { get; }

        string GetProjectWorkingDirectory(string projectName);
    }

    public class WorkingDirectoryRootProvider : IWorkingDirectoryRootProvider
    {
        public string WorkingDirectoryRoot
        {
            get
            {
                var workingDirectoryRoot = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Gundi",
                    "WorkingDirectory");
                return workingDirectoryRoot;
            }
        }

        public string GetProjectWorkingDirectory(string projectName)
        {
            return Path.Combine(WorkingDirectoryRoot, projectName);
        }
    }
}