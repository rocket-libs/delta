using System;
using Rocket.Libraries.Delta.RemoteRepository;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public class ProjectDefinition
    {
        public string Label { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectPath { get; set; }
        
        public RepositoryDetail RepositoryDetail { get; set; }
    }
}