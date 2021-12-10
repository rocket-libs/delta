using System.IO;
using System.Threading.Tasks;
using Rocket.Libraries.Delta.ProcessRunning;
using Rocket.Libraries.Delta.ProjectDefinitions;

namespace Rocket.Libraries.Delta.PreExecutionTasks.InBuilt
{
    public interface IWorkingDirectoryRootCreator : IPreExecutionTasks
    {
        
    }

    public class WorkingDirectoryRootCreator : IWorkingDirectoryRootCreator
    {
        private readonly IWorkingDirectoryRootProvider workingDirectoryRootProvider;

        public WorkingDirectoryRootCreator(
            IWorkingDirectoryRootProvider workingDirectoryRootProvider
        )
        {
            this.workingDirectoryRootProvider = workingDirectoryRootProvider;
        }

        public Task ExecuteAsync(ProjectDefinition projectDefinition)
        {
            if(!Directory.Exists(workingDirectoryRootProvider.WorkingDirectoryRoot))
            {
                Directory.CreateDirectory(workingDirectoryRootProvider.WorkingDirectoryRoot);
            }
            return Task.CompletedTask;
        }
    }
}