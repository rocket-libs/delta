using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace delta
{
    public class Program
    {

        const int port = 5002;
        public static string DefaultPath => $"http://localhost:{port}/index.html";
        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.ClearProviders();
                            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                            logging.SetMinimumLevel(LogLevel.Trace);
                            logging.AddConsole();
                            logging.AddDebug();
                            logging.AddEventSourceLogger();
                        })
                        .UseNLog()
                        .UseContentRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                        .UseUrls($"http://0.0.0.0:{port}");
                });
        }

        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                LaunchWebUIIfRequested(args);
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception e)
            {
                logger.Error(e, "Unhandled exception caught in the global try-catch block... This is bad");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        private static bool HasArg(string[] args, string arg)
        {
            if (args == null || args.Length == 0)
            {
                return false;
            }
            return args.Any(a => a.Equals(arg, StringComparison.InvariantCulture));
        }

        private static void LaunchWebUIIfRequested(string[] args)
        {
            if (HasArg(args, "--webui"))
            {

                Process.Start(new ProcessStartInfo
                {
                    FileName = $"http://localhost:{port}/index.html",
                    UseShellExecute = true
                });
            }
        }
    }
}