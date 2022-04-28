using System;
using Rocket.Libraries.Delta.RemoteRepository;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public class ProjectDefinition
    {
        private bool keepSource;

        public string Label { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectPath { get; set; }

        public RepositoryDetail RepositoryDetail { get; set; }

        public bool HasNoRemoteRepository => RepositoryDetail == default;

        public bool KeepSource 
        { 
            get
            {
                if(HasNoRemoteRepository)
                {
                    return true;
                }
                else
                {
                    return keepSource;
                }
            }
            
            set => keepSource = value; 
        }
    }
}