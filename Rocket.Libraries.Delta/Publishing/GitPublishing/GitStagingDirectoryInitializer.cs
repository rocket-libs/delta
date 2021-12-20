using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.Delta.GitInterfacing;
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
        Task EnsureLocalRepositoryReadyAsync(Project project, IGitInterface gitInterface);
    }

    public class GitStagingDirectoryInitializer : IGitStagingDirectoryInitializer
    {
        private readonly IFileSystemAccessor fileSystemAccessor;
        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public GitStagingDirectoryInitializer(
            IStagingDirectoryResolver stagingDirectoryResolver,
            IFileSystemAccessor fileSystemAccessor
            )
        {
            this.stagingDirectoryResolver = stagingDirectoryResolver;
            this.fileSystemAccessor = fileSystemAccessor;

        }

        public async Task EnsureLocalRepositoryReadyAsync(Project project, IGitInterface gitInterface)
        {
            await gitInterface.SetupAsync(
                workingDirectory: stagingDirectoryResolver.GetProjectStagingDirectory(project),
                projectId: project.Id,
                branch: project.Branch,
                url: project.PublishUrl);
            var workingDirectoryIsNotGitRepository = await gitInterface.WorkingDirectoryIsGitRepositoryAsync() == false;
            if (workingDirectoryIsNotGitRepository)
            {
                await gitInterface.CloneAsync();
                await EnsureLocalRepositoryReadyAsync(project, gitInterface);
            }
            else
            {
                await gitInterface.DiscardAllChangesAsync();
            }
            if (await gitInterface.RemoteBranchExistsAsync())
            {
                await gitInterface.CheckOutBranchAsync();
                await gitInterface.PullAsync();
            }
            else
            {
                await gitInterface.CreateBranchAsync();
                await gitInterface.SetUpstreamBranch();
            }
            /*await fileSystemAccessor.WriteAllTextAsync($"{projectStagingDirectory}/README.md", "Gundi Release Repository\n\nEdit this file to describe your project");
            await InitializeRepository();
            await AddRemoteAsync(project);
            await StageAllAsync();
            await DoInitialCommitAsync(project);
            await TrackBranchAsync();*/

        }




        // private async Task AddRemoteAsync(Project project)
        // {
        //     await gitInterface.AddRemoteAsync(project.PublishUrl);
        // }

        // private async Task TrackBranchAsync()
        // {
        //     /*var processStartInformation = new ProcessStartInformation
        //     {
        //         Filename = "git",
        //         Arguments = $"push -u origin master",
        //         WorkingDirectory = stagingDirectoryResolver.GetProjectStagingDirectory(project),
        //         Timeout = TimeSpan.FromMinutes(2)
        //     };
        //     await processRunner.RunAsync(processStartInformation,project.Id);*/
        //     await gitInterface.TrackBranchAsync();
        // }

        // private async Task DoInitialCommitAsync(Project project)
        // {
        //     await gitInterface.CommitAsync($"Initial commit for {project.Id}");
        // }

        // private async Task InitializeRepository()
        // {
        //     await gitInterface.InitializeRepositoryAsync();
        // }

        // private async Task StageAllAsync()
        // {
        //     await gitInterface.StageAllAsync();
        // }
    }
}