using System;
using System.IO;
using System.Linq;
using delta.Publishing;
using Rocket.Libraries.Delta.ProcessRunning;
using Rocket.Libraries.Delta.Projects;

namespace delta.Running
{
    public interface IStagingDirectoryResolver
    {
        string GetProjectStagingDirectory(Project project);
    }

    public class StagingDirectoryResolver : IStagingDirectoryResolver
    {
        private readonly IProjectStagingDirectoryResolver projectStagingDirectoryResolver;
        private readonly IWorkingDirectoryRootProvider workingDirectoryRootProvider;

        public StagingDirectoryResolver(
            IProjectStagingDirectoryResolver projectStagingDirectoryResolver,
            IWorkingDirectoryRootProvider workingDirectoryRootProvider)
        {
            this.projectStagingDirectoryResolver = projectStagingDirectoryResolver;
            this.workingDirectoryRootProvider = workingDirectoryRootProvider;
        }

        

        public string GetProjectStagingDirectory(Project project)
        {
            var stagingDirectory = Path.Combine(
                workingDirectoryRootProvider.WorkingDirectoryRoot
            , projectStagingDirectoryResolver.GetProjectStagingDirectoryName(project));
            if(Directory.Exists(stagingDirectory))
            {
                var children = Directory.GetDirectories(stagingDirectory);
                if(children != null)
                {
                    if(children.Length > 1)
                    {
                        throw new Exception($"More than one directory found in {stagingDirectory}. Only the local repository should be present.");
                    }
                    else if(children.Length == 1)
                    {
                        return children[0];
                    }
                }
            }
            return stagingDirectory;
        }
    }
}