using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.Delta.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace delta.Publishing.GitPublishing
{
    public interface IGitStagingDirectoryInitializer
    {
        Task EnsureLocalRepositoryReadyAsync(Project project);
    }

    public class GitStagingDirectoryInitializer : IGitStagingDirectoryInitializer
    {
        private readonly IFileSystemAccessor fileSystemAccessor;

        private readonly IProcessRunner processRunner;

        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public GitStagingDirectoryInitializer(
            IStagingDirectoryResolver stagingDirectoryResolver,
            IProcessRunner processRunner,
            IFileSystemAccessor fileSystemAccessor
            )
        {
            this.stagingDirectoryResolver = stagingDirectoryResolver;
            this.processRunner = processRunner;
            this.fileSystemAccessor = fileSystemAccessor;
        }

        public async Task EnsureLocalRepositoryReadyAsync(Project project)
        {
            var projectStagingDirectory = stagingDirectoryResolver.GetStagingDirectory(project);
            if (!Directory.Exists(projectStagingDirectory))
            {
                Directory.CreateDirectory(projectStagingDirectory);
                await fileSystemAccessor.WriteAllTextAsync($"{projectStagingDirectory}/README.md", "Delta Repository\n\nEdit this file to describe your project");
                await InitializeRepository(project);
                await AddRemoteAsync(project);
                await StageAllAsync(project);
                await DoInitialCommitAsync(project);
                await ConnectToRemoteAsync(project);
            }
        }

        private async Task AddRemoteAsync(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = $"remote add origin {project.PublishUrl}",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(2)
            };
            await processRunner.RunAsync(processStartInformation);
        }

        private async Task ConnectToRemoteAsync(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = $"push -u origin master",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(2)
            };
            await processRunner.RunAsync(processStartInformation);
        }

        private async Task DoInitialCommitAsync(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = $"commit -m \"initial commit\"",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(2)
            };
            await processRunner.RunAsync(processStartInformation);
        }

        private async Task InitializeRepository(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = $"init",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(2)
            };
            await processRunner.RunAsync(processStartInformation);
        }

        private async Task StageAllAsync(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = "add -A",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(1)
            };
            await processRunner.RunAsync(processStartInformation);
        }
    }
}