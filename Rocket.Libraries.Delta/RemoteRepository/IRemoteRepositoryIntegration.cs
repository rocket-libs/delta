using System.Threading.Tasks;
using Rocket.Libraries.Delta.ProjectDefinitions;

namespace Rocket.Libraries.Delta.RemoteRepository
{
    public interface IRemoteRepositoryIntegration
    {
        Task GetAsync(ProjectDefinition projectDefinition);

        
    }
}