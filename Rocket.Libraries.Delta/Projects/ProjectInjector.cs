using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.Delta.ProjectDefinitions;
using Rocket.Libraries.Delta.RemoteRepository;

namespace Rocket.Libraries.Delta.Projects
{
    public interface IProjectInjector
    {
        Task InjectAsync(ProjectDefinition projectDefinition);
        Task<string> ResolveProjectPathAsync(ProjectDefinition projectDefinition);
    }

    public class ProjectInjector : IProjectInjector
    {
        private readonly IFileSystemAccessor fileSystemAccessor;
        private readonly IGitRemoteRepositoryIntegration gitRemoteRepositoryIntegration;

        public ProjectInjector(
            IFileSystemAccessor fileSystemAccessor,
            IGitRemoteRepositoryIntegration gitRemoteRepositoryIntegration
        )
        {
            this.fileSystemAccessor = fileSystemAccessor;
            this.gitRemoteRepositoryIntegration = gitRemoteRepositoryIntegration;
        }

        public async Task<string> ResolveProjectPathAsync(ProjectDefinition projectDefinition)
        {
            var projectPath = await gitRemoteRepositoryIntegration.GetFullProjectPath(projectDefinition);
            return projectPath;
        }
        public async Task InjectAsync(ProjectDefinition projectDefinition)
        {
            var projectPath = await ResolveProjectPathAsync(projectDefinition);

            if (fileSystemAccessor.FileExists(projectPath))
            {

                var projectText = await fileSystemAccessor.GetAllTextAsync(projectPath);
                projectDefinition.Project = JsonSerializer.Deserialize<Project>(projectText);
                projectDefinition.Project.OnFailurePostBuildCommands = projectDefinition.Project.OnFailurePostBuildCommands ?? ImmutableList<BuildCommand>.Empty;
                projectDefinition.Project.OnSuccessPostBuildCommands = projectDefinition.Project.OnSuccessPostBuildCommands ?? ImmutableList<BuildCommand>.Empty;
                projectDefinition.Project.BuildCommands = projectDefinition.Project.BuildCommands ?? ImmutableList<string>.Empty;
                projectDefinition.Project.DisabledStages = projectDefinition.Project.DisabledStages ?? ImmutableHashSet<string>.Empty;
            }
            else
            {
                projectDefinition.Project = new Project();
            }
        }
    }
}