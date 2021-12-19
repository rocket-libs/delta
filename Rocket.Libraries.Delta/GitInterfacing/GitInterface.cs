using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using delta.Running;

namespace Rocket.Libraries.Delta.GitInterfacing
{
    public interface IGitInterface
    {
        Task CheckOutBranchAsync(string branch);
        Task DiscardAllChangesAsync();
        void Initialize(string workingDirectory, Guid projectId);
        Task PullAsync();
        Task PushAsync();
    }

    public class GitInterface : IGitInterface
    {
        private readonly IExternalProcessRunner externalProcessRunner;

        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        private GitInterfaceCommand gitInterfaceCommand;

        public GitInterface(
            IExternalProcessRunner externalProcessRunner
        )
        {
            this.externalProcessRunner = externalProcessRunner;
        }


        public void Initialize(string workingDirectory, Guid projectId)
        {
            gitInterfaceCommand = new GitInterfaceCommand()
            {
                WorkingDirectory = workingDirectory,
                ProjectId = projectId
            };
        }

        public async Task DiscardAllChangesAsync()
        {
            gitInterfaceCommand.Command = "git reset --hard";
            await RunGitCommand(gitInterfaceCommand);
        }

        public async Task CheckOutBranchAsync(string branch)
        {
            gitInterfaceCommand.Command = $"git checkout {branch}";
            await RunGitCommand(gitInterfaceCommand);
        }

        public async Task PullAsync()
        {
            gitInterfaceCommand.Command = "git pull";
            await RunGitCommand(gitInterfaceCommand);
        }

        public async Task PushAsync()
        {
            gitInterfaceCommand.Command = "git push";
            await RunGitCommand(gitInterfaceCommand);
        }


        private async Task RunGitCommand(GitInterfaceCommand gitInterfaceCommand)
        {
            try
            {
                await semaphore.WaitAsync();
                FailIfNotInitializedCorrectly(gitInterfaceCommand);
                await externalProcessRunner.RunExternalProcessAsync(
                    command: gitInterfaceCommand.Command,
                    workingDirectory: gitInterfaceCommand.WorkingDirectory,
                    projectId: gitInterfaceCommand.ProjectId);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void FailIfNotInitializedCorrectly(GitInterfaceCommand gitInterfaceCommand)
        {
            FailIfProjectIdNotInitializedCorrectly(gitInterfaceCommand);
            FailIfWorkingDirectoryNotInitializedCorrectly(gitInterfaceCommand);
        }

        private void FailIfWorkingDirectoryNotInitializedCorrectly(GitInterfaceCommand gitInterfaceCommand)
        {
            if (string.IsNullOrEmpty(gitInterfaceCommand.WorkingDirectory))
            {
                throw new Exception($"Working directory for git command '{gitInterfaceCommand.Command}' not set");
            }
            if (!Directory.Exists(gitInterfaceCommand.WorkingDirectory))
            {
                throw new Exception($"Working directory for git command '{gitInterfaceCommand.Command}' does not exist");
            }
        }

        private void FailIfProjectIdNotInitializedCorrectly(GitInterfaceCommand gitInterfaceCommand)
        {
            if (gitInterfaceCommand.ProjectId == Guid.Empty)
            {
                throw new Exception($"ProjectId for git command '{gitInterfaceCommand.Command}' not set");
            }
        }
    }
}