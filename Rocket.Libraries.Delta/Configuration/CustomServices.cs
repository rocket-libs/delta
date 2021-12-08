using delta.ProcessRunning;
using delta.Publishing;
using delta.Publishing.GitPublishing;
using delta.Running;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Libraries.CallProxying.Services;
using Rocket.Libraries.Delta.Configuration.Routing;
using Rocket.Libraries.Delta.EventStreaming;
using Rocket.Libraries.Delta.ExtensionsHelper;
using Rocket.Libraries.Delta.FileSystem;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
using Rocket.Libraries.Delta.ProcessRunning;
using Rocket.Libraries.Delta.ProjectDefinitions;
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
                .AddScoped<IOutputsCopier, OutputsCopier> ()
                .AddScoped<IFileSystemAccessor, FileSystemAccessor> ()
                .AddScoped<IProjectDefinitionWriter, ProjectDefinitionWriter> ()
                .AddScoped<IProcessRunner, ProcessRunner> ()
                .AddScoped<IReleasePublisher, GitPublisher> ()
                .AddScoped<IStagingDirectoryResolver, StagingDirectoryResolver> ()
                .AddScoped<IProjectStagingDirectoryResolver, GitProjectStagingDirectoryResolver> ()
                .AddScoped<IGitStagingDirectoryInitializer, GitStagingDirectoryInitializer> ()
                .AddScoped<IGitReponseVerifier, GitReponseVerifier> ()
                .AddScoped<IExternalProcessRunner, ExternalProcessRunner> ()
                .AddScoped<IExtensionHelper, ExtensionHelper> ()
                .AddScoped<IProcessRunnerLoggerBuilder, ProcessRunnerLoggerBuilder> ()
                .AddScoped<IProcessFilenameResolver, ProcessFilenameResolver> ()
                .AddSingleton<IEventStreamer, EventStreamer> ();
        }
    }
}