using System;
using Rocket.Libraries.Delta.Projects;
using Rocket.Libraries.Delta.RemoteRepository;
using Rocket.Libraries.FormValidationHelper.Attributes.InBuilt.Guids;
using Rocket.Libraries.FormValidationHelper.Attributes.InBuilt.Objects;
using Rocket.Libraries.FormValidationHelper.Attributes.InBuilt.Strings;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public class ProjectDefinition
    {
        private bool keepSource;

        [StringIsNonNullable("Project Label")]
        public string Label { get; set; }

        [GuidHasNonDefaultValue("Project Id")]
        public Guid ProjectId { get; set; }

        [StringIsNonNullable("Path To Project")]
        public string ProjectPath { get; set; }

        public RepositoryDetail RepositoryDetail { get; set; }

        public bool HasNoRemoteRepository => RepositoryDetail == default;

        public Project Project {get; set;}

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

        public string PublishUrl { get; set; }
    }
}