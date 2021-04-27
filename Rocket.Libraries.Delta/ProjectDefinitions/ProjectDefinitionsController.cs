using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocket.Libraries.CallProxying.Models;
using Rocket.Libraries.CallProxying.Services;
using Rocket.Libraries.Delta.Configuration.Routing;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public class ProjectDefinitionsController : DeltaController
    {
        private readonly IProjectDefinitionWriter projectDefinitionWriter;
        private readonly IProjectDefinitionsReader projectDefinitionsReader;

        public ProjectDefinitionsController (
            ICallProxy callProxy,
            IProjectDefinitionWriter projectDefinitionWriter,
            IProjectDefinitionsReader projectDefinitionsReader) : base (callProxy)
        {
            this.projectDefinitionWriter = projectDefinitionWriter;
            this.projectDefinitionsReader = projectDefinitionsReader;
        }

        [HttpPost ("insert")]
        public async Task<WrappedResponse<ProjectDefinition>> InsertAsync ([FromBody] ProjectDefinition projectDefinition)
        {
            using (CallProxy)
            {
                return await CallProxy.CallAsync (async () => await projectDefinitionWriter.InsertAsync (projectDefinition));
            }
        }

        [HttpGet ("get-all")]
        public async Task<WrappedResponse<ImmutableList<ProjectDefinition>>> GetAllAsync ()
        {
            using (CallProxy)
            {
                return await CallProxy.CallAsync (async () => await projectDefinitionsReader.GetAllProjectDefinitionsAsync ());
            }
        }
    }
}