using delta.Publishing;
using Rocket.Libraries.Delta.Projects;

namespace delta.Running
{
    public interface IStagingDirectoryResolver
    {
        string StagingRootDirectory { get; }

        string GetStagingDirectory(Project project);
    }

    public class StagingDirectoryResolver : IStagingDirectoryResolver
    {
        private readonly IProjectStagingDirectoryResolver projectStagingDirectoryResolver;

        public StagingDirectoryResolver(
            IProjectStagingDirectoryResolver projectStagingDirectoryResolver)
        {
            this.projectStagingDirectoryResolver = projectStagingDirectoryResolver;
        }

        public string StagingRootDirectory => $"./staging-directory/";

        public string GetStagingDirectory(Project project)
        {
            return $"{StagingRootDirectory}{projectStagingDirectoryResolver.GetProjectStagingDirectoryName(project)}/";
        }
    }
}