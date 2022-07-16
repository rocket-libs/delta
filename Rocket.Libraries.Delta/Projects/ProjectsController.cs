using Rocket.Libraries.CallProxying.Services;
using Rocket.Libraries.Delta.Configuration.Routing;

namespace Rocket.Libraries.Delta.Projects
{
    public class ProjectsController : DeltaController
    {
        public ProjectsController(
            ICallProxy callProxy)
             : base(callProxy)
        {
        }

        public async Task<WrappedResponse<Project>> ByIdAsync()
        {
            
        }
    }
}