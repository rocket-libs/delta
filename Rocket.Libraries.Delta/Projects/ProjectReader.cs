using System;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.EventStreaming;
using Rocket.Libraries.Delta.ProjectDefinitions;
using Rocket.Libraries.Delta.RemoteRepository;

namespace Rocket.Libraries.Delta.Projects
{
    public interface IProjectReader
    {
        Project GetByPath (string projectPath, Guid projectId, string branch);

        Task<Project> GetByProjectDefinitionAsync (ProjectDefinition projectDefinition);

        Task<Project> GetByIdAsync (Guid id);
    }

    public class ProjectReader : IProjectReader
    {
        private readonly IEventQueue eventQueue;
        private readonly IGitRemoteRepositoryIntegration gitRemoteRepositoryIntegration;
        private readonly IProjectDefinitionsReader projectDefinitionsReader;

        public ProjectReader (
            IEventQueue eventQueue,
            IGitRemoteRepositoryIntegration gitRemoteRepositoryIntegration,
            IProjectDefinitionsReader projectDefinitionsReader
        )
        {
            this.eventQueue = eventQueue;
            this.gitRemoteRepositoryIntegration = gitRemoteRepositoryIntegration;
            this.projectDefinitionsReader = projectDefinitionsReader;
        }

        public async Task<Project> GetByIdAsync (Guid id)
        {
            var projectDefinition = await projectDefinitionsReader.GetSingleProjectDefinitionByIdAsync (id);
            return await GetByProjectDefinitionAsync (projectDefinition);
        }
        public async Task<Project> GetByProjectDefinitionAsync (ProjectDefinition projectDefinition)
        {
            try
            {
                await gitRemoteRepositoryIntegration.ExecuteAsync (projectDefinition);
                return GetByPath (
                    projectDefinition.ProjectPath,
                    projectDefinition.ProjectId,
                    projectDefinition.RepositoryDetail.Branch);
            }
            catch (Exception e)
            {
                await this.eventQueue.EnqueueSingleAsync (projectDefinition.ProjectId, e.Message);
                return null;
            }
            
        }

        public Project GetByPath (
            string projectPath, Guid projectId, string branch)
        {
            if (!File.Exists (projectPath))
            {
                eventQueue.EnqueueSingleAsync (projectId, $"Error: Project file not found at {projectPath}");
                return new Project
                {
                    Label = "??Missing Project??",
                };
            }
            using (var fileStream = new FileStream (projectPath, FileMode.Open))
            {
                using (var streamReader = new StreamReader (fileStream))
                {
                    var project = JsonSerializer.Deserialize<Project> (streamReader.ReadToEnd ());
                    project.DisabledStages = project.DisabledStages ?? ImmutableHashSet<string>.Empty;
                    project.Id = projectId;
                    project.Branch = branch;
                    project.PublishUrl = project.PublishUrl ?? string.Empty;
                    return project;
                }
            }
        }
    }
}