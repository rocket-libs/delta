using Rocket.Libraries.Delta.Projects;

namespace delta.Publishing
{
    public interface IProjectStagingDirectoryResolver
    {
        string GetProjectStagingDirectoryName(Project project);
    }
}