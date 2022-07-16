using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocket.Libraries.CallProxying.Models;
using Rocket.Libraries.CallProxying.Services;
using Rocket.Libraries.Delta.Configuration.Routing;

namespace Rocket.Libraries.Delta.Projects
{
    public class ProjectsController : DeltaController
    {
        private readonly IProjectReader projectReader;

        public ProjectsController (
            ICallProxy callProxy,
            IProjectReader projectReader) : base (callProxy)
        {
            this.projectReader = projectReader;
        }

        [HttpGet ("get-by-id")]
        public async Task<WrappedResponse<Project>> ByIdAsync ([FromQuery] Guid id)
        {
            using (CallProxy)
            {
                return await CallProxy.CallAsync (async () => await projectReader.GetByIdAsync (id));
            }
        }
    }
}