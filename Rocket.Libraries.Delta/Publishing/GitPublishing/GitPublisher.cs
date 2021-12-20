using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.EventStreaming;
using Rocket.Libraries.Delta.GitInterfacing;
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
        private readonly IGitInterface gitInterface;
        private readonly IEventQueue eventQueue;
        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public GitPublisher(
            IStagingDirectoryResolver stagingDirectoryResolver,
            IGitStagingDirectoryInitializer gitStagingDirectoryInitializer,
            IGitReponseVerifier gitReponseVerifier,
            IExternalProcessRunner externalProcessRunner,
            IProcessRunnerLoggerBuilder processRunnerLoggerBuilder,
            IGitInterface gitInterface,
            IEventQueue eventQueue)
        {
            this.stagingDirectoryResolver = stagingDirectoryResolver;
            this.gitStagingDirectoryInitializer = gitStagingDirectoryInitializer;
            this.gitReponseVerifier = gitReponseVerifier;
            this.externalProcessRunner = externalProcessRunner;
            this.processRunnerLoggerBuilder = processRunnerLoggerBuilder;
            this.gitInterface = gitInterface;
            this.eventQueue = eventQueue;
        }

        public async Task PrepareOutputDirectoryAsync(Project project)
        {
            gitInterface.Initialize(
                workingDirectory: stagingDirectoryResolver.GetProjectStagingDirectory(project),
                projectId: project.Id,
                branch: project.Branch,
                url: project.PublishUrl);
            await gitStagingDirectoryInitializer.EnsureLocalRepositoryReadyAsync(project, gitInterface);
        }

        public async Task PublishAsync(Project project)
        {
            var tag = await GetTagAsync(project);
            await gitInterface.StageAllAsync();
            await gitInterface.CommitAsync($"Release {tag}");
            await TagReleaseAsync(tag);
            await gitInterface.PushAsync();
            await gitInterface.PushTagsAsync();
        }

        

        private async Task<long> GetTagAsync(Project project)
        {
            try
            {
                await externalProcessRunner.RunExternalProcessAsync(
                    command: "git describe",
                    workingDirectory: stagingDirectoryResolver.GetProjectStagingDirectory(project),
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

        private async Task TagReleaseAsync(long tag)
        {
            await gitInterface.TagCommitAsync(tag.ToString());
        }
    }
}