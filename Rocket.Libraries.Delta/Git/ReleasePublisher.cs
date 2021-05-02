using delta.ProcessRunning;
using delta.Running;
using Rocket.Libraries.Delta.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delta.Git
{
    public interface IReleasePublisher
    {
        Task PublishAsync(Project project);
    }

    public class ReleasePublisher : IReleasePublisher
    {
        private readonly IProcessRunner processRunner;

        private readonly IStagingDirectoryResolver stagingDirectoryResolver;

        public ReleasePublisher(
            IProcessRunner processRunner,
            IStagingDirectoryResolver stagingDirectoryResolver)
        {
            this.processRunner = processRunner;
            this.stagingDirectoryResolver = stagingDirectoryResolver;
        }

        public async Task PublishAsync(Project project)
        {
            var tag = await GetTagAsync(project);
            await StageAllAsync(project);
            await CommitAllAsync(project, tag);
            await TagReleaseAsync(project, tag);
            await PushReleaseAsync(project);
            await PushTagsAsync(project);
        }

        private async Task CommitAllAsync(Project project, long tag)
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

        private async Task<long> GetTagAsync(Project project)
        {
            var processStartInformation = new ProcessStartInformation
            {
                Filename = "git",
                Arguments = "describe",
                WorkingDirectory = stagingDirectoryResolver.GetStagingDirectory(project),
                Timeout = TimeSpan.FromMinutes(1)
            };
            var results = await processRunner.RunAsync(processStartInformation);
            var latestTag = default(long);
            if (results.StandardOutput != null || results.StandardOutput.Length == 1)
            {
                latestTag = long.Parse(results.StandardOutput[0]);
            }
            return ++latestTag;
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