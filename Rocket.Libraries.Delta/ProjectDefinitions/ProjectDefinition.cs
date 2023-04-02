using System.Reflection.Emit;
using System;
using System.Text.Json.Serialization;
using Rocket.Libraries.Delta.Projects;
using Rocket.Libraries.Delta.RemoteRepository;
using Rocket.Libraries.FormValidationHelper.Attributes.InBuilt.Guids;
using Rocket.Libraries.FormValidationHelper.Attributes.InBuilt.Objects;
using Rocket.Libraries.FormValidationHelper.Attributes.InBuilt.Strings;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public class ProjectDefinition
    {
        //private bool keepSource;
        private Project project = new Project();

        public bool HasNoRemoteRepository => RepositoryDetail == default;

        public bool KeepSource
        {
            get
            {
                if (HasNoRemoteRepository)
                {
                    return true;
                }
                else
                {
                    if (project != null)
                    {
                        return project.KeepSource;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            set => Project.KeepSource = value;
        }

        [StringIsNonNullable("Project Label")]
        public string Label { get; set; }

        public Project Project
        {
            get
            {
                if (project != null)
                {
                    if (string.IsNullOrEmpty(project.Branch) && RepositoryDetail != default)
                    {
                        project.Branch = RepositoryDetail.Branch;
                    }

                    if (project.Id == default)
                    {
                        project.Id = ProjectId;
                    }

                    if (string.IsNullOrEmpty(project.Label))
                    {
                        project.Label = Label;
                    }
                }

                return project;
            }

            set => project = value;
        }

        [GuidHasNonDefaultValue("Project Id")]
        public Guid ProjectId { get; set; }

        [StringIsNonNullable("Path To Project")]
        public string ProjectPath { get; set; }

        public string PublishUrl { get; set; }

        public RepositoryDetail RepositoryDetail { get; set; }

        [JsonIgnore]
        internal bool HasRemoteRepository => !HasNoRemoteRepository;
    }
}