using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.EventStreaming;
using Rocket.Libraries.Delta.GitInterfacing;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
using Rocket.Libraries.Delta.Projects;
using Rocket.Libraries.Delta.Running;

namespace delta.Publishing.GitPublishing
{
    public class GitPublisher : IReleasePublisher
    {
        private readonly IExternalProcessRunner externalProcessRunner;

        private readonly IGitStagingDirectoryInitializer gitStagingDirectoryInitializer;

        private readonly IGitInterface gitInterface;
        private readonly IEventQueue eventQueue;
        private readonly IProcessResponseParser processResponseParser;
        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public GitPublisher (
            IStagingDirectoryResolver stagingDirectoryResolver,
            IGitStagingDirectoryInitializer gitStagingDirectoryInitializer,
            IExternalProcessRunner externalProcessRunner,
            IGitInterface gitInterface,
            IEventQueue eventQueue,
            IProcessResponseParser processResponseParser)
        {
            this.stagingDirectoryResolver = stagingDirectoryResolver;
            this.gitStagingDirectoryInitializer = gitStagingDirectoryInitializer;
            this.externalProcessRunner = externalProcessRunner;
            this.gitInterface = gitInterface;
            this.eventQueue = eventQueue;
            this.processResponseParser = processResponseParser;
        }

        public async Task PrepareOutputDirectoryAsync (Project project)
        {
            await gitInterface.SetupAsync (
                workingDirectory: stagingDirectoryResolver.GetProjectStagingDirectory (project),
                projectId: project.Id,
                branch: project.Branch,
                url: project.PublishUrl);
            await gitStagingDirectoryInitializer.EnsureLocalRepositoryReadyAsync (project, gitInterface);
        }

        public async Task PublishAsync (Project project)
        {
            var tag = await GetTagAsync (project);
            await gitInterface.StageAllAsync ();
            var commitMessage = $"Release Tagged '{tag}' On Branch '{project.Branch}'";
            try
            {
                await gitInterface.CommitAsync (commitMessage);
                await gitInterface.TagCommitAsync ($"{project.Branch}-{tag}");
            }
            catch
            {
                if (processResponseParser.OutputIsNot("nothing to commit, working tree clean"))
                {
                    throw;
                }
                else
                {
                    await eventQueue.EnqueueSingleAsync (
                        project.Id,
                        $"No changes detected. Commit '{commitMessage}' will be rolled back.");
                }
            }
            await gitInterface.PushAsync ();
            await gitInterface.PushTagsAsync ();
        }

        private async Task<long> GetTagAsync (Project project)
        {
            var fullTag = await gitInterface.GetLatestTagAsync ();
            var latestTag = default (long);
            if (!string.IsNullOrEmpty (fullTag))
            {
                var tag = fullTag.Split ('-').Last ();
                latestTag = long.Parse (tag);
            }
            return ++latestTag;
        }

        /*private async Task<bool> IsCommitRequiredAsync(Project project)
        {
            const string noCommitMarker = "Your branch is up to date";
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = "status",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory (project),
                Timeout = TimeSpan.FromMinutes (2)
            };
            var results = await processRunner.RunAsync (processStartInformation);
            if (results.StandardOutput != null)
            {
                foreach (var item in results.StandardOutput)
                {
                    var notEmpty = !string.IsNullOrEmpty (item);
                    var hasSufficientLength = item.Length > noCommitMarker.Length;
                    var startsWithSearchString = item.Trim ().StartsWith (noCommitMarker, StringComparison.InvariantCultureIgnoreCase);
                    if (notEmpty && hasSufficientLength && startsWithSearchString)
                    {
                        return false;
                    }
                }
            }
            return true;
        }*/

    }
}