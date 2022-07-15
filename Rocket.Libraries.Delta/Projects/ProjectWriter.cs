using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.FormValidationHelper;
using Rocket.Libraries.FormValidationHelper.Attributes;

namespace Rocket.Libraries.Delta.Projects
{
    public interface IProjectWriter
    {
        Task<ValidationResponse<Project>> WriteAsync(Project project, string filename);
    }

    public class ProjectWriter : IProjectWriter
    {
        private readonly IFileSystemAccessor fileSystemAccessor;
        private readonly IValidationResponseHelper validationResponseHelper;

        public ProjectWriter(
            IFileSystemAccessor fileSystemAccessor,
            IValidationResponseHelper validationResponseHelper)
        {
            this.fileSystemAccessor = fileSystemAccessor;
            this.validationResponseHelper = validationResponseHelper;
        }

        public async Task<ValidationResponse<Project>> WriteAsync(
            Project project,
            string filename)
        {
            using (var validator = new BasicFormValidator<Project>())
            {
                var validationResponse = await validator.ValidateAndPackAsync(project);
                return await validationResponseHelper.RouteResponseAsync(
                    validationResponse,
                    async (_) => await WriteValidatedAsync(project, filename)
                );
            }
        }

        private async Task<ValidationResponse<Project>> WriteValidatedAsync(
            Project project,
            string filename)
        {
            await fileSystemAccessor.WriteAllTextAsync(
                filename,
                JsonSerializer.Serialize(
                    project,
                    options: new JsonSerializerOptions
                    {
                        WriteIndented = true,

                    }));
            return validationResponseHelper.SuccessValue(project);
        }
    }
}