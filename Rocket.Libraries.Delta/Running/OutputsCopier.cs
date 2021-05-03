using System.IO;
using System.Threading.Tasks;
using delta.Publishing;
using delta.Running;
using Rocket.Libraries.Delta.Projects;

namespace Rocket.Libraries.Delta.Running
{
    public interface IOutputsCopier
    {
        Task CopyOutputsAsync(string projectPath, Project project);
    }

    public class OutputsCopier : IOutputsCopier
    {
        private readonly IReleasePublisher releasePublisher;

        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public OutputsCopier(
            IReleasePublisher releasePublisher,
            IStagingDirectoryResolver stagingDirectoryResolver

            )
        {
            this.releasePublisher = releasePublisher;
            this.stagingDirectoryResolver = stagingDirectoryResolver;
        }

        public async Task CopyOutputsAsync(string projectPath, Project project)
        {
            var outputsDirectory = $"{Path.GetDirectoryName(projectPath)}/{project.BuildOutputDirectory}";
            if (!Directory.Exists(stagingDirectoryResolver.StagingRootDirectory))
            {
                Directory.CreateDirectory(stagingDirectoryResolver.StagingRootDirectory);
            }
            await releasePublisher.PrepareOutputDirectoryAsync(project);
            var stagingDirectory = stagingDirectoryResolver.GetStagingDirectory(project);
            CopyAll(new DirectoryInfo(outputsDirectory), new DirectoryInfo(stagingDirectory));
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}