using delta.ProcessRunning;
using Rocket.Libraries.Delta.Projects;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace delta.Publishing
{
    public interface IReleasePublisher
    {
        Task PrepareOutputDirectoryAsync(Project project);

        Task PublishAsync(Project project);
    }
}