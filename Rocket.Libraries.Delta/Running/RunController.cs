using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using delta.ProcessRunning;
using Microsoft.AspNetCore.Mvc;
using Rocket.Libraries.CallProxying.Models;
using Rocket.Libraries.CallProxying.Services;
using Rocket.Libraries.Delta.Configuration.Routing;

namespace Rocket.Libraries.Delta.Running
{
    public class RunController : DeltaController
    {
        private readonly IRunner runner;

        public RunController(
            ICallProxy callProxy,
            IRunner runner) : base(callProxy)
        {
            this.runner = runner;
        }

        [HttpGet("run-by-id")]
        public async Task<WrappedResponse<ImmutableList<ProcessRunningResults>>> RunByIdAsync([FromQuery] Guid projectId)
        {
            using (CallProxy)
            {
                return await CallProxy.CallAsync(async () => await runner.RunAsync(projectId));
            }
        }
    }
}