using System;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using Rocket.Libraries.Delta.EventStreaming;

namespace Rocket.Libraries.Delta.Projects
{
    public interface IProjectReader
    {
        Project GetByPath(string projectPath, Guid projectId);
    }

    public class ProjectReader : IProjectReader
    {
        private readonly IEventQueue eventQueue;

        public ProjectReader(
            IEventQueue eventQueue
        )
        {
            this.eventQueue = eventQueue;
        }

        public Project GetByPath(
            string projectPath, Guid projectId)
        {
            if (!File.Exists(projectPath))
            {
                eventQueue.EnqueueSingleAsync(projectId, $"Error: Project file not found at {projectPath}");
                return new Project
                {
                    Label = "??Missing Project??",
                };
            }
            using (var fileStream = new FileStream(projectPath, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var project = JsonSerializer.Deserialize<Project>(streamReader.ReadToEnd());
                    project.DisabledStages = project.DisabledStages ?? ImmutableHashSet<string>.Empty;
                    project.Id = projectId;
                    return project;
                }
            }
        }
    }
}