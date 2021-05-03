using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.FileSystem;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public interface IProjectDefinitionWriter
    {
        Task<ProjectDefinition> InsertAsync(ProjectDefinition projectDefinition);
    }

    public class ProjectDefinitionWriter : ProjectDefinitionStoreAccessor, IProjectDefinitionWriter
    {
        private readonly IProjectDefinitionsReader projectDefinitionsReader;
        private readonly IFileSystemAccessor fileSystemAccessor;

        public ProjectDefinitionWriter(
            IProjectDefinitionsReader projectDefinitionsReader,
            IFileSystemAccessor fileSystemAccessor)
        {
            this.projectDefinitionsReader = projectDefinitionsReader;
            this.fileSystemAccessor = fileSystemAccessor;
        }

        public async Task<ProjectDefinition> InsertAsync(ProjectDefinition projectDefinition)
        {
            try
            {
                var allProjectDefinitions = await projectDefinitionsReader.GetAllProjectDefinitionsAsync();
                await Semaphore.WaitAsync();
                var collision = allProjectDefinitions.Any(a => a.ProjectPath.Equals(projectDefinition.ProjectPath));
                if (collision)
                {
                    throw new Exception($"Project at path '{projectDefinition.ProjectPath}' has already been added");
                }
                projectDefinition.ProjectId = Guid.NewGuid();
                allProjectDefinitions = allProjectDefinitions.Add(projectDefinition);
                await fileSystemAccessor.WriteAllTextAsync(ProjectsDefinitionStoreFile, JsonSerializer.Serialize(allProjectDefinitions));
                return projectDefinition;
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}