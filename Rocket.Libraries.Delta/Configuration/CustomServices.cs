using Microsoft.Extensions.DependencyInjection;
using Rocket.Libraries.CallProxying.Services;
using Rocket.Libraries.Delta.Configuration.Routing;
using Rocket.Libraries.Delta.ProjectRegistrar;
using Rocket.Libraries.Delta.Projects;
using Rocket.Libraries.Delta.Running;

namespace Rocket.Libraries.Delta.Configuration
{
    public static class CustomServices
    {
        public static void ConfigureCustomServices (this IServiceCollection services)
        {
            services.AddScoped<IProjectDefinitionsReader, ProjectDefinitionsReader> ()
                .AddScoped<IProjectReader, ProjectReader> ()
                .AddScoped<IRunner, Runner> ()
                .AddScoped<ICallProxy, Rocket.Libraries.CallProxying.Services.CallProxy> ()
                .AddScoped<IProxyActions, ProxyAction> ()
                .AddScoped<IProjectValidator, ProjectValidator> ()
                .AddScoped<IOutputsCopier, OutputsCopier> ();
        }
    }
}