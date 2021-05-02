using Rocket.Libraries.Delta.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delta.Running
{
    public interface IStagingDirectoryResolver
    {
        string GetStagingDirectory(Project project);
    }

    public class StagingDirectoryResolver : IStagingDirectoryResolver
    {
        public string GetStagingDirectory(Project project)
        {
            return $"./staging-directory/{project.Label}/";
        }
    }
}