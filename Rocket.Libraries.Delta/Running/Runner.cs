using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using delta.ProcessRunning;
using delta.Publishing;
using delta.Running;
using Rocket.Libraries.Delta.ProjectDefinitions;
using Rocket.Libraries.Delta.Projects;

namespace Rocket.Libraries.Delta.Running
{
    public interface IRunner
    {
        Task<ImmutableList<ProcessRunningResults>> RunAsync(Guid projectId);
    }

    public class Runner : IRunner
    {
        private readonly IExternalProcessRunner externalProcessRunner;

        private readonly IOutputsCopier outputsCopier;

        private readonly IProjectDefinitionsReader projectDefinitionsReader;

        private readonly IProjectReader projectReader;

        private readonly IProjectValidator projectValidator;

        private readonly IReleasePublisher releasePublisher;

        public Runner(
            IProjectDefinitionsReader projectDefinitionsReader,
            IProjectReader projectReader,
            IProjectValidator projectValidator,
            IOutputsCopier outputsCopier,
            IReleasePublisher releasePublisher,
            IExternalProcessRunner externalProcessRunner
        )
        {
            this.projectDefinitionsReader = projectDefinitionsReader;
            this.projectReader = projectReader;
            this.projectValidator = projectValidator;
            this.outputsCopier = outputsCopier;
            this.releasePublisher = releasePublisher;
            this.externalProcessRunner = externalProcessRunner;
        }

        public async Task<ImmutableList<ProcessRunningResults>> RunAsync(Guid projectId)
        {
            var projectDefinition = await projectDefinitionsReader.GetSingleProjectDefinitionByIdAsync(projectId);
            projectValidator.FailIfProjectInvalid(projectDefinition, projectId);
            var project = projectReader.GetByPath(projectDefinition.ProjectPath);
            if (project == default)
            {
                throw new Exception($"Could not load project at path '{projectDefinition.ProjectPath}'");
            }
            var results = await RunCommandsAsync(projectDefinition, project);
            await outputsCopier.CopyOutputsAsync(projectDefinition.ProjectPath, project);
            results = await releasePublisher.PublishAsync(project, results);
            return results;
        }

        private void RedirectToStandardOutputsIfNotUsingShellExecute(Process process)
        {
            if (process.StartInfo.UseShellExecute)
            {
                return;
            }
            else
            {
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
            }
        }

        private async Task<ImmutableList<ProcessRunningResults>> RunCommandsAsync(ProjectDefinition projectDefinition, Project project)
        {
            var workingDirectory = Path.GetDirectoryName(projectDefinition.ProjectPath);
            var results = ImmutableList<ProcessRunningResults>.Empty;
            for (var i = 0; i < project.BuildCommands.Count; i++)
            {
                results = results.Add(await externalProcessRunner.RunExternalProcessAsync(project.BuildCommands[i], workingDirectory));
            }
            return results;
        }
    }
}