using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.Delta.Projects;
using Rocket.Libraries.FormValidationHelper;
using Rocket.Libraries.FormValidationHelper.Attributes;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public interface IProjectDefinitionWriter
    {
        Task<ValidationResponse<ProjectDefinition>> InsertAsync(ProjectDefinition projectDefinition);

        Task<ValidationResponse<ProjectDefinition>> UpdateAsync(ProjectDefinition projectDefinition);
    }

    public class ProjectDefinitionWriter : ProjectDefinitionStoreAccessor, IProjectDefinitionWriter
    {
        private readonly IProjectDefinitionsReader projectDefinitionsReader;
        private readonly IFileSystemAccessor fileSystemAccessor;
        private readonly IProjectWriter projectWriter;
        private readonly IValidationResponseHelper validationResponseHelper;

        public const string ErrorKeyExistingProjectPath = "ErrorKeyExistingProjectPath";
        public const string ErrorKeyExistingLabel = "ErrorKeyExistingLabel";

        public ProjectDefinitionWriter(
            IProjectDefinitionsReader projectDefinitionsReader,
            IFileSystemAccessor fileSystemAccessor,
            IProjectWriter projectWriter,
            IValidationResponseHelper validationResponseHelper)
        {
            this.projectDefinitionsReader = projectDefinitionsReader;
            this.fileSystemAccessor = fileSystemAccessor;
            this.projectWriter = projectWriter;
            this.validationResponseHelper = validationResponseHelper;
        }

        public async Task<ValidationResponse<ProjectDefinition>> InsertAsync(ProjectDefinition projectDefinition)
        {
            return await WriteAsync(
                projectDefinition,
                isInsert: true
            );
        }

        public async Task<ValidationResponse<ProjectDefinition>> UpdateAsync(ProjectDefinition projectDefinition)
        {
            return await WriteAsync(
                projectDefinition,
                isInsert: false
            );
        }

        private async Task<ValidationResponse<ProjectDefinition>> WriteAsync(
            ProjectDefinition projectDefinition,
            bool isInsert)
        {
            try
            {
                var allProjectDefinitions = await projectDefinitionsReader.GetAllProjectDefinitionsAsync();
                await Semaphore.WaitAsync();
                if (isInsert)
                {
                    var collision = !string.IsNullOrEmpty(projectDefinition.ProjectPath) && allProjectDefinitions.Any(a => a.ProjectPath.ToLower().Equals(projectDefinition.ProjectPath.ToLower()));
                    if (collision)
                    {
                        return validationResponseHelper.TypedError<ProjectDefinition>(
                            $"Project at path '{projectDefinition.ProjectPath}' has already been added",
                            ErrorKeyExistingProjectPath
                        );
                    }
                    collision = allProjectDefinitions.Any(a => a.Label.ToLower().Equals(projectDefinition.Label.ToLower()));
                    if (collision)
                    {
                        return validationResponseHelper.TypedError<ProjectDefinition>(
                            $"Project with label '{projectDefinition.Label}' has already been added",
                            ErrorKeyExistingLabel
                        );
                    }
                    projectDefinition.ProjectId = Guid.NewGuid();
                    if (projectDefinition.Project != null)
                    {
                        projectDefinition.Project.Id = projectDefinition.ProjectId;
                    }
                }
                else
                {
                    allProjectDefinitions = allProjectDefinitions.Where(a => a.ProjectId != projectDefinition.ProjectId).ToImmutableList();
                }

                using (var validator = new BasicFormValidator<ProjectDefinition>())
                {
                    var validationResponse = await validator.ValidateAndPackAsync(projectDefinition);
                    if (validationResponse.HasErrors)
                    {
                        return validationResponse;
                    }
                    else
                    {
                        return await WriteValidatedAsync(projectDefinition, allProjectDefinitions);
                    }
                }

            }
            finally
            {
                Semaphore.Release();
            }
        }

        private async Task<ValidationResponse<ProjectDefinition>> WriteValidatedAsync(
            ProjectDefinition projectDefinition,
            ImmutableList<ProjectDefinition> allProjectDefinitions)
        {
            if (await WriteProjectAsync(projectDefinition))
            {
                allProjectDefinitions = allProjectDefinitions.Add(projectDefinition);
                await fileSystemAccessor.WriteAllTextAsync(ProjectsDefinitionStoreFile, JsonSerializer.Serialize(allProjectDefinitions));
                return new ValidationResponse<ProjectDefinition>
                {
                    Entity = projectDefinition,
                };
            }
            else
            {
                return validationResponseHelper.TypedError<ProjectDefinition>(
                    "Unable to save configuration",
                    "ErrorKeyProjectSaveError"
                );
            }
        }

        private async Task<bool> WriteProjectAsync(ProjectDefinition projectDefinition)
        {
            if (projectDefinition.Project == null)
            {
                return true;
            }
            else
            {
                var projectWriteResult = await projectWriter.WriteAsync(
                    projectDefinition
                );
                projectDefinition.PublishUrl = projectDefinition.Project.PublishUrl;
                projectDefinition.Project = null;
                return projectWriteResult.HasErrors == false;
            }
        }

    }
}