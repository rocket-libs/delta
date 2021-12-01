﻿using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.Projects;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;

namespace delta.Publishing.GitPublishing
{
    public class GitPublisher : IReleasePublisher
    {
        private readonly IExternalProcessRunner externalProcessRunner;

        private readonly IGitReponseVerifier gitReponseVerifier;

        private readonly IGitStagingDirectoryInitializer gitStagingDirectoryInitializer;

        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public GitPublisher(
            IStagingDirectoryResolver stagingDirectoryResolver,
            IGitStagingDirectoryInitializer gitStagingDirectoryInitializer,
            IGitReponseVerifier gitReponseVerifier,
            IExternalProcessRunner externalProcessRunner)
        {
            this.stagingDirectoryResolver = stagingDirectoryResolver;
            this.gitStagingDirectoryInitializer = gitStagingDirectoryInitializer;
            this.gitReponseVerifier = gitReponseVerifier;
            this.externalProcessRunner = externalProcessRunner;
        }

        public async Task PrepareOutputDirectoryAsync(Project project)
        {
            await gitStagingDirectoryInitializer.EnsureLocalRepositoryReadyAsync(project);
        }

        public async Task<ImmutableList<ProcessRunningResults>> PublishAsync(Project project, ImmutableList<ProcessRunningResults> results)
        {
            results = results.Add(await PullFromRemoteAsync(project));
            var tag = await GetTagAsync(project);
            results = results.Add(await StageAllAsync(project));
            results = results.Add(await CommitAllAsync(project, tag));
            results = results.Add(await TagReleaseAsync(project, tag));
            results = results.Add(await PushReleaseAsync(project));
            results = results.Add(await PushTagsAsync(project));
            return results;
        }

        private async Task<ProcessRunningResults> CommitAllAsync(Project project, long tag)
        {
            try
            {
                return await externalProcessRunner.RunExternalProcessAsync(
                    command: $"git commit -m \"Release {tag}\"",
                    workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project));
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
                var results = await externalProcessRunner.RunExternalProcessAsync(
                    command: "git describe",
                    workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project));
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

        private async Task<ProcessRunningResults> PullFromRemoteAsync(Project project)
        {
            return await externalProcessRunner.RunExternalProcessAsync(
                command: "git pull",
                workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project));
        }

        private async Task<ProcessRunningResults> PushReleaseAsync(Project project)
        {
            return await externalProcessRunner.RunExternalProcessAsync(
                 command: "git push",
                 workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project));
        }

        private async Task<ProcessRunningResults> PushTagsAsync(Project project)
        {
            return await externalProcessRunner.RunExternalProcessAsync(
                command: $"git push origin --tags",
                workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project));
        }

        private async Task<ProcessRunningResults> StageAllAsync(Project project)
        {
            return await externalProcessRunner.RunExternalProcessAsync(
                command: "git add -A",
                workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project));
        }

        private async Task<ProcessRunningResults> TagReleaseAsync(Project project, long tag)
        {
            return await externalProcessRunner.RunExternalProcessAsync(
                command: $"git tag -a {tag} -m {tag}",
                workingDirectory: stagingDirectoryResolver.GetStagingDirectory(project));
        }
    }
}