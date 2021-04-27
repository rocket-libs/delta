using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Rocket.Libraries.CallProxying.Services;

namespace Rocket.Libraries.Delta.Configuration.Routing
{
    [ExcludeFromCodeCoverageAttribute]
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class DeltaController
    {
         public DeltaController(ICallProxy callProxy)
        {
            CallProxy = callProxy;
        }

        protected ICallProxy CallProxy { get; }
    }
}