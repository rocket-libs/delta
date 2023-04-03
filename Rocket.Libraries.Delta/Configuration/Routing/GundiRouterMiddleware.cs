using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Rocket.Libraries.Delta.Configuration.Routing
{
    public class GundiRouterMiddleware
    {
        private readonly RequestDelegate handler;

        public GundiRouterMiddleware(
            RequestDelegate handler
        )
        {
            this.handler = handler;
        }

        public async Task InvokeAsync(
            HttpContext httpContext)
        {
            await handler(httpContext);
            if (httpContext.Response.StatusCode == 404)
            {
                httpContext.Request.Path = "/project-definitions/list";
                await handler(httpContext);
            }
        }
    }
}