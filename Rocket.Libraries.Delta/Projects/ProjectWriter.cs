using System.Text.Json;
using System.Threading.Tasks;
using delta.Publishing;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.Delta.ProjectDefinitions;
using Rocket.Libraries.Delta.RemoteRepository;
using Rocket.Libraries.FormValidationHelper;
using Rocket.Libraries.FormValidationHelper.Attributes;

namespace Rocket.Libraries.Delta.Projects
{
    public interface IProjectWriter
    {
        Task<ValidationResponse<Project>> WriteAsync(ProjectDefinition projectDefinition);
    }

    public class ProjectWriter : IProjectWriter
    {
        private readonly IFileSystemAccessor fileSystemAccessor;

        private readonly IGitRemoteRepositoryIntegration gitRemoteRepositoryIntegration;
        private readonly IProjectInjector projectInjector;
        private readonly IValidationResponseHelper validationResponseHelper;

        public ProjectWriter(
            IFileSystemAccessor fileSystemAccessor,
            IValidationResponseHelper validationResponseHelper,
            IGitRemoteRepositoryIntegration gitRemoteRepositoryIntegration,
            IProjectInjector projectInjector)
        {
            this.fileSystemAccessor = fileSystemAccessor;
            this.validationResponseHelper = validationResponseHelper;
            this.gitRemoteRepositoryIntegration = gitRemoteRepositoryIntegration;
            this.projectInjector = projectInjector;
        }

        public async Task<ValidationResponse<Project>> WriteAsync(
            ProjectDefinition projectDefinition)
        {
            using (var validator = new BasicFormValidator<Project>())
            {
                var validationResponse = await validator.ValidateAndPackAsync(projectDefinition.Project);
                return await validationResponseHelper.RouteResponseAsync(
                    validationResponse,
                    async (_) => await WriteValidatedAsync(projectDefinition)
                );
            }
        }




        private async Task<ValidationResponse<Project>> WriteValidatedAsync(
            ProjectDefinition projectDefinition)
        {
            if (projectDefinition.Project != null)
            {
                var projectPath = await projectInjector.ResolveProjectPathAsync(projectDefinition);
                await fileSystemAccessor.WriteAllTextAsync(
                    projectPath,
                    JsonSerializer.Serialize(
                        projectDefinition.Project,
                        options: new JsonSerializerOptions
                        {
                            WriteIndented = true,
                        }));
                await gitRemoteRepositoryIntegration.SyncAsync(projectDefinition, "Gundi Build Configuration Updated");
            }
            return validationResponseHelper.SuccessValue(projectDefinition.Project);
        }
    }
}