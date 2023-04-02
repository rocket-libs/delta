using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using delta.Running;
using Rocket.Libraries.Delta.EventStreaming;
using Rocket.Libraries.Delta.GitInterfacing;
using Rocket.Libraries.Delta.PreExecutionTasks;
using Rocket.Libraries.Delta.ProcessRunning;
using Rocket.Libraries.Delta.ProjectDefinitions;

namespace Rocket.Libraries.Delta.RemoteRepository
{
    public interface IGitRemoteRepositoryIntegration : IPreExecutionTasks
    {
        Task<string> GetFullProjectPath(ProjectDefinition projectDefinition);

        string GetGitRootFolder(ProjectDefinition projectDefinition);

        Task SyncAsync(ProjectDefinition projectDefinition, string commitMessage);
    }

    public class GitRemoteRepositoryIntegration : IRemoteRepositoryIntegration, IGitRemoteRepositoryIntegration
    {
        private readonly IEventQueue eventQueue;

        private readonly IExternalProcessRunner externalProcessRunner;

        private readonly IGitInterface gitInterface;

        private readonly IWorkingDirectoryRootProvider workingDirectoryRootProvider;

        public GitRemoteRepositoryIntegration(
            IExternalProcessRunner externalProcessRunner,
            IWorkingDirectoryRootProvider workingDirectoryRootProvider,
            IEventQueue eventQueue,
            IGitInterface gitInterface)
        {
            this.externalProcessRunner = externalProcessRunner;
            this.workingDirectoryRootProvider = workingDirectoryRootProvider;
            this.eventQueue = eventQueue;
            this.gitInterface = gitInterface;
        }

        public async Task ExecuteAsync(ProjectDefinition projectDefinition)
        {
            if (projectDefinition.HasNoRemoteRepository)
            {
                await eventQueue.EnqueueSingleAsync(
                    projectDefinition.ProjectId,
                    "Does not have details of a remote repository. Local repository will be used if available.");
                return;
            }
            await GetAsync(projectDefinition);
        }

        public async Task GetAsync(ProjectDefinition projectDefinition)
        {
            var projectWorkingDirectory = GetProjectWorkingDirectory(projectDefinition);

            if (!Directory.Exists(projectWorkingDirectory))
            {
                await InitializeRepositoryAsync(projectDefinition, projectWorkingDirectory);
            }
            else
            {
                await PullChangesAsync(projectDefinition, projectWorkingDirectory);
            }

            projectDefinition.ProjectPath = await GetFullProjectPath(projectDefinition);
        }

        public async Task<string> GetFullProjectPath(ProjectDefinition projectDefinition)
        {
            var projectWorkingDirectory = GetProjectWorkingDirectory(projectDefinition);
            await ShowLastCommitMessageAsync(projectDefinition, projectWorkingDirectory);
            var gitRootFolder = GetGitRootFolder(projectWorkingDirectory);
            return Path.Combine(gitRootFolder, projectDefinition.ProjectPath);
        }

        public string GetGitRootFolder(ProjectDefinition projectDefinition)
        {
            var projectWorkingDirectory = workingDirectoryRootProvider.GetProjectWorkingDirectory(projectDefinition.Label, "Sources");
            return GetGitRootFolder(projectWorkingDirectory);
        }

        public async Task SyncAsync(ProjectDefinition projectDefinition, string commitMessage)
        {
            var projectWorkingDirectory = workingDirectoryRootProvider.GetProjectWorkingDirectory(projectDefinition.Label, "Sources");
            var gitRootFolder = GetGitRootFolder(projectWorkingDirectory);
            if (!string.IsNullOrEmpty(gitRootFolder))
            {
                await externalProcessRunner.RunExternalProcessAsync(
                    $"git add -A",
                    gitRootFolder,
                    projectDefinition.ProjectId);

                await externalProcessRunner.RunExternalProcessAsync(
                    $"git commit -m \"{commitMessage}\"",
                    gitRootFolder,
                    projectDefinition.ProjectId,
                    customSuccessEvaluator: (results) =>
                    {
                        if (results != null && results.StandardError == null || results.StandardError.Length == 0)
                        {
                            if (results.StandardOutput != null && results.StandardOutput.Length >= 2)
                            {
                                var successPrefixes = new List<string>
                                {
                                    "Your branch is ahead of",
                                    "Your branch is up to date with",
                                };
                                foreach (var specificSuccessString in successPrefixes)
                                {
                                    var succeeded = results.StandardOutput[1].StartsWith(specificSuccessString);
                                    if (succeeded)
                                    {
                                        return true;
                                    }
                                }
                                var successSubstrings = new List<string>
                                {
                                    "] Gundi Build Configuration Updated",
                                };
                                foreach (var specificSuccessString in successSubstrings)
                                {
                                    var succeeded = results.StandardOutput[0].Contains(specificSuccessString);
                                    if (succeeded)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        return false;
                    });

                /*await externalProcessRunner.RunExternalProcessAsync(
                    $"git pull",
                    gitRootFolder,
                    projectDefinition.ProjectId);*/

                await externalProcessRunner.RunExternalProcessAsync(
                    $"git fetch --all --tags",
                    gitRootFolder,
                    projectDefinition.ProjectId);

                await externalProcessRunner.RunExternalProcessAsync(
                    $"git checkout {projectDefinition.RepositoryDetail.Branch}",
                    gitRootFolder,
                    projectDefinition.ProjectId);

                await externalProcessRunner.RunExternalProcessAsync(
                    $"git merge -X ours origin/{projectDefinition.RepositoryDetail.Branch}",
                    gitRootFolder,
                    projectDefinition.ProjectId);

                await externalProcessRunner.RunExternalProcessAsync(
                    $"git push",
                    gitRootFolder,
                    projectDefinition.ProjectId);
            }
        }

        private string GetGitRootFolder(string searchDirectory)
        {
            var candidateDirectory = Path.Combine(searchDirectory, ".git");
            if (Directory.Exists(candidateDirectory))
            {
                return searchDirectory;
            }
            else
            {
                foreach (var directory in Directory.GetDirectories(searchDirectory))
                {
                    var nextSearchDirectory = Path.Combine(searchDirectory, directory);
                    var result = GetGitRootFolder(nextSearchDirectory);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
            return string.Empty;
        }

        private string GetProjectWorkingDirectory(ProjectDefinition projectDefinition)
        {
            return workingDirectoryRootProvider.GetProjectWorkingDirectory(projectDefinition.Label, "Sources");
        }

        private async Task InitializeRepositoryAsync(ProjectDefinition projectDefinition, string projectWorkingDirectory)
        {
            Directory.CreateDirectory(projectWorkingDirectory);
            await externalProcessRunner.RunExternalProcessAsync(
                $"git clone --branch {projectDefinition.RepositoryDetail.Branch} {projectDefinition.RepositoryDetail.Url}",
                projectWorkingDirectory,
                projectDefinition.ProjectId);
        }

        private async Task PullChangesAsync(ProjectDefinition projectDefinition, string projectWorkingDirectory)
        {
            var gitRootFolder = GetGitRootFolder(projectWorkingDirectory);
            if (!string.IsNullOrEmpty(gitRootFolder))
            {
                await externalProcessRunner.RunExternalProcessAsync(
                    $"git reset --hard HEAD",
                    gitRootFolder,
                    projectDefinition.ProjectId);

                await externalProcessRunner.RunExternalProcessAsync(
                    $"git checkout {projectDefinition.RepositoryDetail.Branch}",
                    gitRootFolder,
                    projectDefinition.ProjectId);

                await externalProcessRunner.RunExternalProcessAsync(
                    $"git pull origin {projectDefinition.RepositoryDetail.Branch}",
                    gitRootFolder,
                    projectDefinition.ProjectId);
            }
            else
            {
                throw new Exception("Git root folder not found");
            }
        }

        private async Task ShowLastCommitMessageAsync(ProjectDefinition projectDefinition, string projectWorkingDirectory)
        {
            await gitInterface.SetupAsync(
                    workingDirectory: projectWorkingDirectory,
                    projectId: projectDefinition.ProjectId,
                    branch: projectDefinition.RepositoryDetail.Branch,
                    url: projectDefinition.RepositoryDetail.Url);
            await eventQueue.EnqueueSingleAsync(projectDefinition.ProjectId, "Last commit message was:-");
            await gitInterface.ShowLatestCommitMessageAsync();
        }
    }
}