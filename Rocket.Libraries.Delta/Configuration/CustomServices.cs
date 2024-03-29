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
using Rocket.Libraries.Delta.GitInterfacing;
using Rocket.Libraries.Delta.PreExecutionTasks;
using Rocket.Libraries.Delta.PreExecutionTasks.InBuilt;
using Rocket.Libraries.Delta.ProcessRunnerLogging;
using Rocket.Libraries.Delta.ProcessRunning;
using Rocket.Libraries.Delta.ProjectDefinitions;
using Rocket.Libraries.Delta.Projects;
using Rocket.Libraries.Delta.RemoteRepository;
using Rocket.Libraries.Delta.Running;
using Rocket.Libraries.Delta.Variables;
using Rocket.Libraries.FormValidationHelper;

namespace Rocket.Libraries.Delta.Configuration
{
    public static class CustomServices
    {
        public static void ConfigureCustomServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IEventQueue, EventQueue>()
                .AddSingleton<IWorkingDirectoryRootProvider, WorkingDirectoryRootProvider>()
                .AddScoped<IProjectDefinitionsReader, ProjectDefinitionsReader>()
                .AddScoped<IProcessRunnerLoggerBuilder, ProcessRunnerLoggerBuilder>()
                .AddScoped<IProjectReader, ProjectReader>()
                .AddScoped<IRunner, Runner>()
                .AddScoped<ICallProxy, Rocket.Libraries.CallProxying.Services.CallProxy>()
                .AddScoped<IProxyActions, ProxyAction>()
                .AddScoped<IProjectValidator, ProjectValidator>()
                .AddScoped<IOutputsCopier, OutputsCopier>()
                .AddScoped<IFileSystemAccessor, FileSystemAccessor>()
                .AddScoped<IProjectDefinitionWriter, ProjectDefinitionWriter>()
                .AddScoped<IProcessRunner, ProcessRunner>()
                .AddScoped<IReleasePublisher, GitPublisher>()
                .AddScoped<IStagingDirectoryResolver, StagingDirectoryResolver>()
                .AddScoped<IProjectStagingDirectoryResolver, GitProjectStagingDirectoryResolver>()
                .AddScoped<IGitStagingDirectoryInitializer, GitStagingDirectoryInitializer>()
                .AddScoped<IProcessResponseParser, ProcessResponseParser>()
                .AddScoped<IExternalProcessRunner, ExternalProcessRunner>()
                .AddScoped<IExtensionHelper, ExtensionHelper>()
                .AddScoped<IProcessFilenameResolver, ProcessFilenameResolver>()
                .AddScoped<IGitRemoteRepositoryIntegration, GitRemoteRepositoryIntegration>()
                .AddScoped<IPreExecutionTasksRunner, PreExecutionTasksRunner>()
                .AddScoped<IWorkingDirectoryRootCreator, WorkingDirectoryRootCreator>()
                .AddScoped<IVariableManager, VariableManager>()
                .AddScoped<IProjectWriter, ProjectWriter>()
                .AddScoped<IValidationResponseHelper, ValidationResponseHelper>()
                .AddScoped<IProjectInjector, ProjectInjector>()

                .AddTransient<IGitInterface, GitInterface>();


        }
    }
}