using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.Projects;
using System;
using System.IO;
using System.Threading.Tasks;

namespace delta.Publishing.GitPublishing
{
    public class GitPublisher : IReleasePublisher
    {
        private readonly IGitReponseVerifier gitReponseVerifier;

        private readonly IGitStagingDirectoryInitializer gitStagingDirectoryInitializer;

        private readonly IProcessRunner processRunner;

        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public GitPublisher(
            IProcessRunner processRunner,
            IStagingDirectoryResolver stagingDirectoryResolver,
            IGitStagingDirectoryInitializer gitStagingDirectoryInitializer,
            IGitReponseVerifier gitReponseVerifier)
        {
            this.processRunner = processRunner;
            this.stagingDirectoryResolver = stagingDirectoryResolver;
            this.gitStagingDirectoryInitializer = gitStagingDirectoryInitializer;
            this.gitReponseVerifier = gitReponseVerifier;
        }

        public async Task PrepareOutputDirectoryAsync(Project project)
        {
            await gitStagingDirectoryInitializer.EnsureLocalRepositoryReadyAsync(project);
        }

        public async Task PublishAsync(Project project)
        {
            await PullFromRemoteAsync(project);
            var tag = await GetTagAsync(project);
            await StageAllAsync(project);
            await CommitAllAsync(project, tag);
            await TagReleaseAsync(project, tag);
            await PushReleaseAsync(project);
            await PushTagsAsync(project);
        }

        private async Task CommitAllAsync(Project project, long tag)
        {
            try
            {
                var processStartInformation = new ProcessStartInformation
                {
                    Filename = "git",
                    Arguments = $"commit -m \"Release {tag}\"",
                    WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                    Timeout = TimeSpan.FromMinutes(1)
                };
                await processRunner.RunAsync(processStartInformation);
            }
            catch (ProcessRunningException processRunningException)
            {
                throw;
            }
        }

        private async Task<long> GetTagAsync(Project project)
        {
            try
            {
                var processStartInformation = new ProcessStartInformation
                {
                    Filename = "git",
                    Arguments = "describe",
                    WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                    Timeout = TimeSpan.FromMinutes(2)
                };
                var results = await processRunner.RunAsync(processStartInformation);
                var latestTag = default(long);
                if (results.StandardOutput != null || results.StandardOutput.Length == 1)
                {
                    latestTag = long.Parse(results.StandardOutput[0]);
                }
                return ++latestTag;
            }
            catch (ProcessRunningException projectRunningException)
            {
                var noTagsYet = gitReponseVerifier.Is("fatal: No names found, cannot describe anything.", projectRunningException.ProcessRunningResults.StandardError);
                if (noTagsYet)
                {
                    return 1;
                }
                else
                {
                    throw;
                }
            }
        }

        /*private async Task<bool> IsCommitRequiredAsync(Project project)
        {
            const string noCommitMarker = "Your branch is up to date";
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = "status",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(2)
            };
            var results = await processRunner.RunAsync(processStartInformation);
            if (results.StandardOutput != null)
            {
                foreach (var item in results.StandardOutput)
                {
                    var notEmpty = !string.IsNullOrEmpty(item);
                    var hasSufficientLength = item.Length > noCommitMarker.Length;
                    var startsWithSearchString = item.Trim().StartsWith(noCommitMarker, StringComparison.InvariantCultureIgnoreCase);
                    if (notEmpty && hasSufficientLength && startsWithSearchString)
                    {
                        return false;
                    }
                }
            }
            return true;
        }*/

        private async Task PullFromRemoteAsync(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = "pull",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(1)
            };
            await processRunner.RunAsync(processStartInformation);
        }

        private async Task PushReleaseAsync(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = $"push",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(5)
            };
            await processRunner.RunAsync(processStartInformation);
        }

        private async Task PushTagsAsync(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = $"push origin --tags",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(5)
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

        private async Task TagReleaseAsync(Project project, long tag)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = $"tag -a {tag} -m {tag}",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(1)
            };
            await processRunner.RunAsync(processStartInformation);
        }
    }
}