using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.Running;

namespace Rocket.Libraries.Delta.GitInterfacing
{
    public interface IGitInterface
    {
        Task AddRemoteAsync (string remoteUrl);
        Task CheckOutBranchAsync ();
        Task CloneAsync ();
        Task CommitAsync (string message);

        Task DiscardAllChangesAsync ();
        void Initialize (string workingDirectory, Guid projectId, string branch, string url);
        Task InitializeRepositoryAsync ();
        Task PullAsync ();
        Task PushAsync ();
        Task PushTagsAsync ();
        Task<bool> RemoteBranchExistsAsync ();
        Task StageAllAsync ();
        Task TagCommitAsync (string tag);
        Task SetUpstreamBranch ();
        Task<bool> WorkingDirectoryIsGitRepositoryAsync ();
        Task CreateBranchAsync ();
        Task FetchAsync ();
        Task<string> GetLatestTagAsync ();
    }

    public class GitInterface : IGitInterface
    {
        private const string Origin = "origin";
        private readonly IExternalProcessRunner externalProcessRunner;
        private readonly IProcessResponseParser processResponseParser;
        private SemaphoreSlim semaphore = new SemaphoreSlim (1, 1);

        private GitInterfaceCommand gitInterfaceCommand;

        public GitInterface (
            IExternalProcessRunner externalProcessRunner,
            IProcessResponseParser processResponseParser
        )
        {
            this.externalProcessRunner = externalProcessRunner;
            this.processResponseParser = processResponseParser;
        }

        public void Initialize (
            string workingDirectory,
            Guid projectId,
            string branch,
            string url)
        {
            gitInterfaceCommand = new GitInterfaceCommand ()
            {
                WorkingDirectory = workingDirectory,
                ProjectId = projectId,
                Branch = branch,
                Url = url
            };
        }

        public async Task<string> GetLatestTagAsync ()
        {
            try
            {
                gitInterfaceCommand.Command = "describe --tags --abbrev=0";
                await RunGitCommandAsync ();
                return processResponseParser.PeekLastResult;
            }
            catch (Exception ex)
            {
                if (
                    processResponseParser.ErrorIsOneOf(
                        "fatal: No names found, cannot describe anything."
                        ,"fatal: No tags can describe '"))
                {
                    return $"{gitInterfaceCommand.Branch}-0";
                }
                throw new Exception ($"Failed to get latest tag for project {gitInterfaceCommand.ProjectId}", ex);
            }
        }

        public async Task FetchAsync ()
        {
            gitInterfaceCommand.Command = $"fetch {Origin}";
            await RunGitCommandAsync ();
        }

        public async Task<bool> RemoteBranchExistsAsync ()
        {
            await FetchAsync ();
            try
            {
                gitInterfaceCommand.Command = $"branch -r --contains {gitInterfaceCommand.Branch}";
                await RunGitCommandAsync ();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task CloneAsync ()
        {
            gitInterfaceCommand.Command = $"clone {gitInterfaceCommand.Url}";
            await RunGitCommandAsync ();
        }

        public async Task<bool> WorkingDirectoryIsGitRepositoryAsync ()
        {
            try
            {
                gitInterfaceCommand.Command = "branch";
                await RunGitCommandAsync ();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SetUpstreamBranch ()
        {
            gitInterfaceCommand.Command = $"push --set-upstream {Origin} {gitInterfaceCommand.Branch}";
            await RunGitCommandAsync ();
        }

        public async Task AddRemoteAsync (string remoteUrl)
        {
            gitInterfaceCommand.Command = $"remote add {Origin} {remoteUrl}";
            await RunGitCommandAsync ();
        }

        public async Task TagCommitAsync (string tag)
        {
            gitInterfaceCommand.Command = $"tag -a {tag} -m {tag}";
            await RunGitCommandAsync ();
        }

        public async Task CommitAsync (string message)
        {
            gitInterfaceCommand.Command = $"commit -m \"{message}\"";
            await RunGitCommandAsync ();
        }

        public async Task StageAllAsync ()
        {
            gitInterfaceCommand.Command = "add -A";
            await RunGitCommandAsync ();
        }

        public async Task DiscardAllChangesAsync ()
        {
            gitInterfaceCommand.Command = "reset --hard";
            await RunGitCommandAsync ();
        }

        public async Task CheckOutBranchAsync ()
        {
            gitInterfaceCommand.Command = $"checkout {gitInterfaceCommand.Branch}";
            await RunGitCommandAsync ();
        }

        public async Task InitializeRepositoryAsync ()
        {
            gitInterfaceCommand.Command = "init";
            await RunGitCommandAsync ();
        }

        public async Task PullAsync ()
        {
            gitInterfaceCommand.Command = "pull";
            await RunGitCommandAsync ();
        }

        public async Task PushAsync ()
        {
            gitInterfaceCommand.Command = "push";
            await RunGitCommandAsync ();
        }

        public async Task PushTagsAsync ()
        {
            gitInterfaceCommand.Command = $"push {Origin} --tags";
            await RunGitCommandAsync ();
        }

        private async Task<ProcessRunningResults> RunGitCommandAsync ()
        {
            try
            {
                await semaphore.WaitAsync ();
                FailIfNotInitializedCorrectly ();
                return await externalProcessRunner.RunExternalProcessAsync (
                    command: $"git {gitInterfaceCommand.Command}",
                    workingDirectory : gitInterfaceCommand.WorkingDirectory,
                    projectId : gitInterfaceCommand.ProjectId);
            }
            finally
            {
                semaphore.Release ();
            }
        }

        private void FailIfNotInitializedCorrectly ()
        {
            FailIfProjectIdNotInitializedCorrectly ();
            FailIfWorkingDirectoryNotInitializedCorrectly ();
            FailIfBranchNotInitializedCorrectly ();
        }

        public void FailIfUrlNotInitializedCorrectly ()
        {
            if (string.IsNullOrEmpty (gitInterfaceCommand.Url))
            {
                throw new Exception ("Url not initialized correctly");
            }
        }

        private void FailIfWorkingDirectoryNotInitializedCorrectly ()
        {
            if (string.IsNullOrEmpty (gitInterfaceCommand.WorkingDirectory))
            {
                throw new Exception ($"Working directory for git command '{gitInterfaceCommand.Command}' not set");
            }
            if (!Directory.Exists (gitInterfaceCommand.WorkingDirectory))
            {
                throw new Exception ($"Working directory for git command '{gitInterfaceCommand.Command}' does not exist");
            }
        }

        private void FailIfBranchNotInitializedCorrectly ()
        {
            if (string.IsNullOrEmpty (gitInterfaceCommand.Branch))
            {
                throw new Exception ($"Branch for git command '{gitInterfaceCommand.Command}' not set");
            }
        }

        private void FailIfProjectIdNotInitializedCorrectly ()
        {
            if (gitInterfaceCommand.ProjectId == Guid.Empty)
            {
                throw new Exception ($"ProjectId for git command '{gitInterfaceCommand.Command}' not set");
            }
        }

        public async Task CreateBranchAsync ()
        {
            gitInterfaceCommand.Command = $"checkout -b {gitInterfaceCommand.Branch}";
            await RunGitCommandAsync ();
        }
    }
}