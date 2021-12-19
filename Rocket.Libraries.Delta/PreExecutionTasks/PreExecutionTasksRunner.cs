using System.Collections.Immutable;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.PreExecutionTasks.InBuilt;
using Rocket.Libraries.Delta.ProjectDefinitions;
using Rocket.Libraries.Delta.RemoteRepository;

namespace Rocket.Libraries.Delta.PreExecutionTasks
{
    public interface IPreExecutionTasksRunner
    {
        Task RunPreExecutionTasksAsync(ProjectDefinition projectDefinition);
    }

    public class PreExecutionTasksRunner : IPreExecutionTasksRunner
    {
        private ImmutableList<IPreExecutionTasks> preExecutionTasks;

        public PreExecutionTasksRunner(
            IGitRemoteRepositoryIntegration gitRemoteRepositoryIntegration,
            IWorkingDirectoryRootCreator workingDirectoryRootCreator
        )
        {
            preExecutionTasks = ImmutableList<IPreExecutionTasks>.Empty
                .Add(workingDirectoryRootCreator)
                .Add(gitRemoteRepositoryIntegration);
        }


        public async Task RunPreExecutionTasksAsync(ProjectDefinition projectDefinition)
        {
            foreach (var preExecutionTask in preExecutionTasks)
            {
                await preExecutionTask.ExecuteAsync(projectDefinition);
            }
        }
    }
}