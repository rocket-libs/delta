using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
using Rocket.Libraries.Delta.Projects;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace delta.Publishing.GitPublishing
{
    public class GitPublisher : IReleasePublisher
    {
        private readonly IExternalProcessRunner externalProcessRunner;

        private readonly IGitReponseVerifier gitReponseVerifier;

        private readonly IGitStagingDirectoryInitializer gitStagingDirectoryInitializer;

        private readonly IProcessRunnerLoggerBuilder processRunnerLoggerBuilder;

        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public GitPublisher(
            IStagingDirectoryResolver stagingDirectoryResolver,
            IGitStagingDirectoryInitializer gitStagingDirectoryInitializer,
            IGitReponseVerifier gitReponseVerifier,
            IExternalProcessRunner externalProcessRunner,
            IProcessRunnerLoggerBuilder processRunnerLoggerBuilder)
        {
            this.stagingDirectoryResolver = stagingDirectoryResolver;
            this.gitStagingDirectoryInitializer = gitStagingDirectoryInitializer;
            this.gitReponseVerifier = gitReponseVerifier;
            this.externalProcessRunner = externalProcessRunner;
            this.processRunnerLoggerBuilder = processRunnerLoggerBuilder;
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
            await externalProcessRunner.RunExternalProcessAsync(
                    command: $"git commit -m \"Release {tag}\"",
                    workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project),
                    project.Id);
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
                await externalProcessRunner.RunExternalProcessAsync(
                    command: "git describe",
                    workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project),
                    project.Id);
                var results = processRunnerLoggerBuilder.Peek.Last();
                var latestTag = default(long);
                if (results.Output != null || results.Output.Length == 1)
                {
                    latestTag = long.Parse(results.Output[0]);
                }
                return ++latestTag;
            }
            catch (ProcessRunningException projectRunningException)
            {
                var noTagsYet = gitReponseVerifier.Is("fatal: No names found, cannot describe anything.", projectRunningException.ProcessRunningResults.Errors);
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
            await externalProcessRunner.RunExternalProcessAsync(
                command: "git pull",
                workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project),
                project.Id);
        }

        private async Task PushReleaseAsync(Project project)
        {
            await externalProcessRunner.RunExternalProcessAsync(
                 command: "git push",
                 workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project),
                 project.Id);
        }

        private async Task PushTagsAsync(Project project)
        {
            await externalProcessRunner.RunExternalProcessAsync(
                command: $"git push origin --tags",
                workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project),
                project.Id);
        }

        private async Task StageAllAsync(Project project)
        {
            await externalProcessRunner.RunExternalProcessAsync(
                command: "git add -A",
                workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project),
                project.Id);
        }

        private async Task TagReleaseAsync(Project project, long tag)
        {
            await externalProcessRunner.RunExternalProcessAsync(
                command: $"git tag -a {tag} -m {tag}",
                workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project),
                project.Id);
        }
    }
}