using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.FileSystem;

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

        public ProjectDefinitionsReader (
            IFileSystemAccessor fileReader)
        {
            this.fileReader = fileReader;
        }

        public async Task<ImmutableList<ProjectDefinition>> GetAllProjectDefinitionsAsync ()
        {
            try
            {
                await Semaphore.WaitAsync ();
                var serializedProjects = await fileReader.GetAllTextAsync (ProjectsDefinitionStoreFile);
                return JsonSerializer.Deserialize<ImmutableList<ProjectDefinition>> (serializedProjects);
            }
            finally
            {
                Semaphore.Release ();
            }
        }

        public async Task<ProjectDefinition> GetSingleProjectDefinitionByIdAsync (Guid projectId)
        {
            var allProjectDefinitions = await GetAllProjectDefinitionsAsync ();
            return allProjectDefinitions.SingleOrDefault (project => project.ProjectId == projectId);
        }
    }
}