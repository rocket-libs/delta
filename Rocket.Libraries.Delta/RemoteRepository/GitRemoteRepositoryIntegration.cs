using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using delta.Running;
using Rocket.Libraries.Delta.EventStreaming;
using Rocket.Libraries.Delta.PreExecutionTasks;
using Rocket.Libraries.Delta.ProcessRunning;
using Rocket.Libraries.Delta.ProjectDefinitions;


namespace Rocket.Libraries.Delta.RemoteRepository
{
    public interface IGitRemoteRepositoryIntegration :  IPreExecutionTasks
    {
        
    }

    public class GitRemoteRepositoryIntegration : IRemoteRepositoryIntegration,  IGitRemoteRepositoryIntegration
    {
        private readonly IExternalProcessRunner externalProcessRunner;
        private readonly IWorkingDirectoryRootProvider workingDirectoryRootProvider;
        private readonly IEventQueue eventQueue;

        public GitRemoteRepositoryIntegration(
            IExternalProcessRunner externalProcessRunner,
            IWorkingDirectoryRootProvider workingDirectoryRootProvider,
            IEventQueue eventQueue)
        {
            this.externalProcessRunner = externalProcessRunner;
            this.workingDirectoryRootProvider = workingDirectoryRootProvider;
            this.eventQueue = eventQueue;
        }

        public async Task ExecuteAsync(ProjectDefinition projectDefinition)
        {
            if(projectDefinition.RepositoryDetail == default)
            {
                await eventQueue.EnqueueAsync(
                    projectDefinition.ProjectId,
                    "Does not have details of a remote repository. Local repository will be used if available.");
                return;
            }
            await GetAsync(projectDefinition);
        }

        public async Task GetAsync(ProjectDefinition projectDefinition)
        {
            var projectWorkingDirectory = workingDirectoryRootProvider.GetProjectWorkingDirectory(projectDefinition.Label);
            if(!Directory.Exists(projectWorkingDirectory))
            {
                await InitializeRepositoryAsync(projectDefinition, projectWorkingDirectory);
            }
            else
            {
                await PullChangesAsync(projectDefinition, projectWorkingDirectory);
            }
            var gitRootFolder = GetGitRootFolder(projectWorkingDirectory);
            projectDefinition.ProjectPath = Path.Combine(gitRootFolder, projectDefinition.ProjectPath);
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
                    $"git reset",
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
                await eventQueue.EnqueueAsync(projectDefinition.ProjectId,"Git root folder not found");
                throw new Exception("Git root folder not found");
            }
        }

        private string GetGitRootFolder(string searchDirectory)
        {
            var candidateDirectory = Path.Combine(searchDirectory, ".git");
           if(Directory.Exists(candidateDirectory))
            {
                return searchDirectory;
            }
            else
            {
                foreach (var directory in Directory.GetDirectories(searchDirectory))
                {
                    var nextSearchDirectory = Path.Combine(searchDirectory, directory);
                    var result = GetGitRootFolder(nextSearchDirectory);
                    if(!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
            return string.Empty;
        }
    }
}