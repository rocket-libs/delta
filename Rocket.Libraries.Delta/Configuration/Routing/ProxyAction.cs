using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rocket.Libraries.CallProxying.Services;

namespace Rocket.Libraries.Delta.Configuration.Routing
{
    public class ProxyAction : IProxyActions
    {
        private readonly ILogger<ProxyAction> logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProxyAction (
            ILogger<ProxyAction> logger,
            IHttpContextAccessor httpContextAccessor)

        {
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Dispose ()
        {

        }

        public Task OnBeforeCallAsync ()
        {
            return Task.CompletedTask;
        }

        public async Task<ImmutableList<object>> OnFailureAsync (Exception exception = null, int httpStatusCode = 400)
        {
            
            logger.LogError (exception, "StackTrace\n{stack}", exception.StackTrace);
            httpContextAccessor.HttpContext.Response.StatusCode = httpStatusCode;
            return await Task.Run (() => default (ImmutableList<object>));
        }

        public Task OnSuccessAsync (object responsePayload)
        {
            return Task.CompletedTask;
        }

        public Task OnTerminatingAsync ()
        {
            return Task.CompletedTask;
        }
    }
}