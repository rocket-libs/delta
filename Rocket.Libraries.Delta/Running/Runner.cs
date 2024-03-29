using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using delta.ProcessRunning;
using delta.Publishing;
using delta.Running;
using Rocket.Libraries.Delta.EventStreaming;
using Rocket.Libraries.Delta.PreExecutionTasks;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
using Rocket.Libraries.Delta.ProcessRunning;
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
        private readonly IEventQueue eventQueue;

        private readonly IExternalProcessRunner externalProcessRunner;

        private readonly IOutputsCopier outputsCopier;

        private readonly IPreExecutionTasksRunner preExecutionTasksRunner;

        private readonly IProcessRunnerLoggerBuilder processRunnerLogger;

        private readonly IProjectDefinitionsReader projectDefinitionsReader;

        private readonly IProjectReader projectReader;

        private readonly IProjectValidator projectValidator;

        private readonly IReleasePublisher releasePublisher;

        private readonly IWorkingDirectoryRootProvider workingDirectoryRootProvider;

        public Runner(
            IProjectDefinitionsReader projectDefinitionsReader,
            IProjectReader projectReader,
            IProjectValidator projectValidator,
            IOutputsCopier outputsCopier,
            IReleasePublisher releasePublisher,
            IExternalProcessRunner externalProcessRunner,
            IProcessRunnerLoggerBuilder processRunnerLogger,
            IEventQueue eventQueue,
            IPreExecutionTasksRunner preExecutionTasksRunner,
            IWorkingDirectoryRootProvider workingDirectoryRootProvider)
        {
            this.processRunnerLogger = processRunnerLogger;
            this.eventQueue = eventQueue;
            this.preExecutionTasksRunner = preExecutionTasksRunner;
            this.workingDirectoryRootProvider = workingDirectoryRootProvider;
            this.projectDefinitionsReader = projectDefinitionsReader;
            this.projectReader = projectReader;
            this.projectValidator = projectValidator;
            this.outputsCopier = outputsCopier;
            this.releasePublisher = releasePublisher;
            this.externalProcessRunner = externalProcessRunner;
        }

        public async Task<ImmutableList<ProcessRunningResults>> RunAsync(Guid projectId)
        {
            var projectDefinition = default(ProjectDefinition);
            var project = default(Project);
            try
            {
                projectDefinition = await projectDefinitionsReader.GetSingleProjectDefinitionByIdAsync(projectId);
                projectValidator.FailIfProjectInvalid(projectDefinition, projectId);
                await preExecutionTasksRunner.RunPreExecutionTasksAsync(projectDefinition);
                project = projectReader.GetByPath(
                    projectDefinition.ProjectPath,
                    projectDefinition.ProjectId,
                    projectDefinition.RepositoryDetail.Branch);
                if (project == default)
                {
                    throw new Exception($"Could not load project at path '{projectDefinition.ProjectPath}'");
                }

                var stageOrder = ImmutableList<string>.Empty
                    .Add(BuildProcessStageNames.RunBuildCommands)
                    .Add(BuildProcessStageNames.CopyToStagingDirectory)
                    .Add(BuildProcessStageNames.PublishToRepository);

                var stages = new Dictionary<string, Func<Task>> {
                    { BuildProcessStageNames.RunBuildCommands, async () => await RunCommandsAsync(projectDefinition, project,project.BuildCommands.Select(a => new BuildCommand
                    {
                        Command = a
                    }).ToImmutableList())},
                    { BuildProcessStageNames.CopyToStagingDirectory, async () => await outputsCopier.CopyOutputsAsync(projectDefinition.ProjectPath, project) },
                    { BuildProcessStageNames.PublishToRepository, async () => await releasePublisher.PublishAsync(project) },
                };

                foreach (var stage in stageOrder)
                {
                    if (project.DisabledStages.Contains(stage))
                    {
                        await processRunnerLogger.LogToOutputAsync($"Skipping stage '{stage}' as it is disabled", projectId);
                    }
                    else
                    {
                        await processRunnerLogger.LogToOutputAsync($"Running stage '{stage}'", projectId);
                        await stages[stage]();
                    }
                }
                await RunPostBuildCommands(projectDefinition, project, project.OnSuccessPostBuildCommands, "Build Success");
                await DeleteSourceIfAllowedAsync(projectDefinition);
            }
            catch (Exception e)
            {
                project = project ?? new Project();
                project.OnFailurePostBuildCommands = project.OnFailurePostBuildCommands ?? ImmutableList<BuildCommand>.Empty;
                await RunPostBuildCommands(projectDefinition, project, project.OnFailurePostBuildCommands, "Build Failure");
                await processRunnerLogger.LogAsync(new ProcessRunningResults
                {
                    Errors = new List<string> { "Unhandled exception", e.Message, e.StackTrace }.ToArray(),
                }, projectId);
            }
            finally
            {
                await processRunnerLogger.LogToOutputAsync("Finished", projectId);
                await eventQueue.CloseAsync(projectId);
            }
            return processRunnerLogger.Build();
        }

        private async Task DeleteSourceIfAllowedAsync(ProjectDefinition projectDefinition)
        {
            var project = await projectReader.GetByIdAsync(projectDefinition.ProjectId);
            if (project.KeepSource || projectDefinition.HasNoRemoteRepository)
            {
                return;
            }
            var projectWorkingDirectory = workingDirectoryRootProvider.GetProjectWorkingDirectory(projectDefinition.Label, "Sources");
            if (Directory.Exists(projectWorkingDirectory))
            {
                try
                {
                    var newDirectoryName = Path.Combine(projectWorkingDirectory, Guid.NewGuid().ToString());
                    Directory.Move(projectWorkingDirectory, newDirectoryName);
                    Directory.Delete(newDirectoryName, true);
                    await eventQueue.EnqueueManyAsync(
                        projectDefinition.ProjectId,
                        new List<string>
                        {
                            $"Deleted source directory '{projectWorkingDirectory}'"
                        });
                }
                catch (Exception ex)
                {
                    await eventQueue.EnqueueManyAsync(
                        projectDefinition.ProjectId,
                        new List<string>
                        {
                            $"Failed to delete source directory '{projectWorkingDirectory}'",
                            ex.Message,
                            "Non-fatal error. Source directory will be left in place."
                        });
                }
            }
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

        private async Task RunCommandsAsync(ProjectDefinition projectDefinition, Project project, ImmutableList<BuildCommand> buildCommands)
        {
            var workingDirectory = Path.GetDirectoryName(projectDefinition.ProjectPath);
            for (var i = 0; i < buildCommands.Count; i++)
            {
                await externalProcessRunner.RunExternalProcessAsync(buildCommands[i], workingDirectory, projectDefinition.ProjectId);
            }
        }

        private async Task RunPostBuildCommands(ProjectDefinition projectDefinition, Project project, ImmutableList<BuildCommand> buildCommands, string mode)
        {
            if (buildCommands != null && buildCommands.Any() && projectDefinition != null && project != null)
            {
                await processRunnerLogger.LogToOutputAsync($"Running {mode} post build commands", project.Id);
                await RunCommandsAsync(projectDefinition, project, buildCommands);
            }
            else
            {
                await processRunnerLogger.LogToOutputAsync($"No {mode} post build commands to run", project.Id);
            }
        }
    }
}