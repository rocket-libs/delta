using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.Delta.Projects;
using Rocket.Libraries.Delta.RemoteRepository;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public interface IProjectDefinitionsReader
    {
        Task<ImmutableList<ProjectDefinition>> GetAllProjectDefinitionsAsync();

        Task<ProjectDefinition> GetSingleProjectDefinitionByIdAsync(Guid projectId);
    }

    public class ProjectDefinitionsReader : ProjectDefinitionStoreAccessor, IProjectDefinitionsReader
    {
        private readonly IFileSystemAccessor fileReader;
        private readonly IProjectInjector projectInjector;

        public ProjectDefinitionsReader(
            IFileSystemAccessor fileReader,
            IProjectInjector projectInjector)
        {
            this.fileReader = fileReader;
            this.projectInjector = projectInjector;
        }

        public async Task<ImmutableList<ProjectDefinition>> GetAllProjectDefinitionsAsync()
        {
            try
            {
                await Semaphore.WaitAsync();
                var serializedProjects = await fileReader.GetAllTextAsync(ProjectsDefinitionStoreFile);
                var projectDefinitions = JsonSerializer.Deserialize<ImmutableList<ProjectDefinition>>(serializedProjects);
                await InjectProjectsAsync(projectDefinitions);
                return projectDefinitions.OrderBy(a => a.Label).ToImmutableList();
            }
            finally
            {
                Semaphore.Release();
            }
        }



        public async Task<ProjectDefinition> GetSingleProjectDefinitionByIdAsync(Guid projectId)
        {
            var allProjectDefinitions = await GetAllProjectDefinitionsAsync();

            var targetProjectDefinition = allProjectDefinitions.SingleOrDefault(project => project.ProjectId == projectId);
            if (targetProjectDefinition == null)
            {
                throw new Exception($"Project with id {projectId} not found");
            }
            await InjectProjectsAsync(new[] { targetProjectDefinition }.ToImmutableList());
            return targetProjectDefinition;
        }
        private async Task InjectProjectsAsync(ImmutableList<ProjectDefinition> projectDefinitions)
        {
            foreach (var projectDefinition in projectDefinitions)
            {
                await projectInjector.InjectAsync(projectDefinition);
            }
        }
    }
}