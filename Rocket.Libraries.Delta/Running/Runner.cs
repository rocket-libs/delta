using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using delta.Publishing;
using Rocket.Libraries.Delta.ProjectDefinitions;
using Rocket.Libraries.Delta.Projects;

namespace Rocket.Libraries.Delta.Running
{
    public interface IRunner
    {
        Task<bool> RunAsync(Guid projectId);
    }

    public class Runner : IRunner
    {
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
            IReleasePublisher releasePublisher
        )
        {
            this.projectDefinitionsReader = projectDefinitionsReader;
            this.projectReader = projectReader;
            this.projectValidator = projectValidator;
            this.outputsCopier = outputsCopier;
            this.releasePublisher = releasePublisher;
        }

        public async Task<bool> RunAsync(Guid projectId)
        {
            var projectDefinition = await projectDefinitionsReader.GetSingleProjectDefinitionByIdAsync(projectId);
            projectValidator.FailIfProjectInvalid(projectDefinition, projectId);
            var project = projectReader.GetByPath(projectDefinition.ProjectPath);
            if (project == default)
            {
                throw new Exception($"Could not load project at path '{projectDefinition.ProjectPath}'");
            }
            RunCommands(projectDefinition, project);
            await outputsCopier.CopyOutputsAsync(projectDefinition.ProjectPath, project);
            await releasePublisher.PublishAsync(project);
            return true;
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

        private void RunCommands(ProjectDefinition projectDefinition, Project project)
        {
            var workingDirectory = Path.GetDirectoryName(projectDefinition.ProjectPath);

            //Directory.SetCurrentDirectory(workingDirectory);
            for (var i = 0; i < project.BuildCommands.Count; i++)
            {
                RunExternalProcess(project.BuildCommands[i], workingDirectory);
            }
        }

        private void RunExternalProcess(string command, string workingDirectory)
        {
            var commandParts = command.Trim().Split(new char[] { ' ' });
            var args = string.Empty;
            var app = commandParts[0];
            var useShellExecute = true;
            if (commandParts.Length > 1)
            {
                for (int i = 1; i < commandParts.Length; i++)
                {
                    args += $" {commandParts[i]}";
                }
            }
            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = args,
                    CreateNoWindow = true,
                    UseShellExecute = useShellExecute,
                    FileName = app,
                    WorkingDirectory = workingDirectory
                }
            })
            {
                //SubscribeToEventsIfUsingShellExecute (process, command.Name);
                process.EnableRaisingEvents = useShellExecute;
                RedirectToStandardOutputsIfNotUsingShellExecute(process);
                process.Start();
                process.WaitForExit();

                //ReadOutputsIfNotUsingShellExecute (process, command.Name);
                //WriteLinesIfAnyCached (command.Name, process.ExitCode);
                if (process.ExitCode != 0)
                {
                    throw new Exception($"Command '{command}' exited with non-success code '{process.ExitCode}'");
                }
            }
        }

        private void SubscribeToEventsIfUsingShellExecute(Process process, string commandName, bool useShellExecute)
        {
            /*var successExitCode = default(string);
            if (useShellExecute)
            {
                process.EnableRaisingEvents = useShellExecute;
                if (successExitCode == null)
                {
                    process.OutputDataReceived += (s, e) => _fnOutput (commandName, e.Data);
                    process.ErrorDataReceived += (s, e) => _fnError (commandName, e.Data);
                }
                else
                {
                    process.OutputDataReceived += (s, e) => CacheLine (commandName, e.Data);
                    process.ErrorDataReceived += (s, e) => CacheLine (commandName, e.Data);
                }
            }*/
        }
    }
}