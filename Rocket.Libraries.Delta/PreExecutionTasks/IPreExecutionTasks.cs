using System.Threading.Tasks;
using Rocket.Libraries.Delta.ProjectDefinitions;

namespace Rocket.Libraries.Delta.PreExecutionTasks
{
    public interface IPreExecutionTasks
    {
        Task ExecuteAsync(ProjectDefinition projectDefinition);
    }
}