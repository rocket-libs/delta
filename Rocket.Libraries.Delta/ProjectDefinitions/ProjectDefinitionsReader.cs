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

        private readonly IProjectReader projectReader;

        public ProjectDefinitionsReader(
            IFileSystemAccessor fileReader,
            IProjectReader projectReader)
        {
            this.fileReader = fileReader;
            this.projectReader = projectReader;
        }

        public async Task<ImmutableList<ProjectDefinition>> GetAllProjectDefinitionsAsync()
        {
            try
            {
                await Semaphore.WaitAsync();
                var serializedProjects = await fileReader.GetAllTextAsync(ProjectsDefinitionStoreFile);
                var projectDefinitions = JsonSerializer.Deserialize<ImmutableList<ProjectDefinition>>(serializedProjects);
                await InjectDisplayLabels(projectDefinitions);
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
            return allProjectDefinitions.SingleOrDefault(project => project.ProjectId == projectId);
        }

        // hvrf63dg6vhkimpbjhn42bqyh2kyvyhdzhluiwmoawloelsiackq
        private async Task InjectDisplayLabels(ImmutableList<ProjectDefinition> projectDefinitions)
        {
            foreach (var specificProjectDefinition in projectDefinitions)
            {
                var project = await projectReader.GetByProjectDefinitionAsync(specificProjectDefinition);
                if (project != null)
                {
                    specificProjectDefinition.Project = project;
                    specificProjectDefinition.PublishUrl = project.PublishUrl;
                }
            }
        }
    }
}