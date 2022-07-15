using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.FormValidationHelper;
using Rocket.Libraries.FormValidationHelper.Attributes;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public interface IProjectDefinitionWriter
    {
        Task<ValidationResponse<ProjectDefinition>> InsertAsync (ProjectDefinition projectDefinition);

        Task<ValidationResponse<ProjectDefinition>> UpdateAsync (ProjectDefinition projectDefinition);
    }

    public class ProjectDefinitionWriter : ProjectDefinitionStoreAccessor, IProjectDefinitionWriter
    {
        private readonly IProjectDefinitionsReader projectDefinitionsReader;
        private readonly IFileSystemAccessor fileSystemAccessor;

        public ProjectDefinitionWriter (
            IProjectDefinitionsReader projectDefinitionsReader,
            IFileSystemAccessor fileSystemAccessor)
        {
            this.projectDefinitionsReader = projectDefinitionsReader;
            this.fileSystemAccessor = fileSystemAccessor;
        }

        public async Task<ValidationResponse<ProjectDefinition>> InsertAsync (ProjectDefinition projectDefinition)
        {
            return await WriteAsync (
                projectDefinition,
                isInsert : true
            );
        }

        public async Task<ValidationResponse<ProjectDefinition>> UpdateAsync (ProjectDefinition projectDefinition)
        {
            return await WriteAsync (
                projectDefinition,
                isInsert : false
            );
        }

        private async Task<ValidationResponse<ProjectDefinition>> WriteAsync (
            ProjectDefinition projectDefinition,
            bool isInsert)
        {
            try
            {
                var allProjectDefinitions = await projectDefinitionsReader.GetAllProjectDefinitionsAsync ();
                await Semaphore.WaitAsync ();
                if (isInsert)
                {
                    var collision = allProjectDefinitions.Any (a => a.ProjectPath.Equals (projectDefinition.ProjectPath));
                    if (collision)
                    {
                        throw new Exception ($"Project at path '{projectDefinition.ProjectPath}' has already been added");
                    }
                    projectDefinition.ProjectId = Guid.NewGuid ();
                }
                else
                {
                    allProjectDefinitions = allProjectDefinitions.Where (a => a.ProjectId != projectDefinition.ProjectId).ToImmutableList ();
                }

                using (var validator = new BasicFormValidator<ProjectDefinition> ())
                {
                    var validationResponse = await validator.ValidateAndPackAsync (projectDefinition);
                    if (validationResponse.HasErrors)
                    {
                        return validationResponse;
                    }
                    else
                    {
                        return await WriteValidatedAsync (projectDefinition, allProjectDefinitions);
                    }
                }

            }
            finally
            {
                Semaphore.Release ();
            }
        }

        private async Task<ValidationResponse<ProjectDefinition>> WriteValidatedAsync (
            ProjectDefinition projectDefinition,
            ImmutableList<ProjectDefinition> allProjectDefinitions)
        {
            allProjectDefinitions = allProjectDefinitions.Add (projectDefinition);
            await fileSystemAccessor.WriteAllTextAsync (ProjectsDefinitionStoreFile, JsonSerializer.Serialize (allProjectDefinitions));
            return new ValidationResponse<ProjectDefinition>
            {
                Entity = projectDefinition,
            };
        }

    }
}