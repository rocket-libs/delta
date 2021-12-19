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
        Task CommitAllAsync(string message);
        Task DiscardAllChangesAsync();
        void Initialize(string workingDirectory, Guid projectId);
        Task PullAsync();
        Task PushAsync();
        Task PushTagsAsync();
        Task StageAllAsync();
        Task TagCommitAsync(string tag);
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

        public async Task TagCommitAsync(string tag)
        {
            gitInterfaceCommand.Command = $"git tag -a {tag} -m {tag}";
            await RunGitCommandAsync();
        }

        public async Task CommitAllAsync(string message)
        {
            gitInterfaceCommand.Command = $"git commit -m \"{message}\"";
            await RunGitCommandAsync();
        }

        public async Task StageAllAsync()
        {
            gitInterfaceCommand.Command = "git add -A";
            await RunGitCommandAsync();
        }

        public async Task DiscardAllChangesAsync()
        {
            gitInterfaceCommand.Command = "git reset --hard";
            await RunGitCommandAsync();
        }

        public async Task CheckOutBranchAsync(string branch)
        {
            gitInterfaceCommand.Command = $"git checkout {branch}";
            await RunGitCommandAsync();
        }

        public async Task PullAsync()
        {
            gitInterfaceCommand.Command = "git pull";
            await RunGitCommandAsync();
        }

        public async Task PushAsync()
        {
            gitInterfaceCommand.Command = "git push";
            await RunGitCommandAsync();
        }

        public async Task PushTagsAsync()
        {
            gitInterfaceCommand.Command = $"git push origin --tags";
            await RunGitCommandAsync();
        }


        private async Task RunGitCommandAsync()
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