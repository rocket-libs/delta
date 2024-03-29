﻿using Rocket.Libraries.Delta.Projects;
using System.IO;

namespace delta.Publishing.GitPublishing
{
    public class GitProjectStagingDirectoryResolver : IProjectStagingDirectoryResolver
    {
        public string GetProjectStagingDirectoryName(Project project)
        {
            var repositoryName = Path.GetFileName(project.PublishUrl);
            return repositoryName;
        }
    }
}