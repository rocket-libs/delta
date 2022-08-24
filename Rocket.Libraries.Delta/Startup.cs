using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Libraries.Delta.Configuration;

namespace delta
{
    public class Startup
    {
        private const string CorsPolicy = "CorsPolicy";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    var routingText = new StringBuilder()
                            .Append($"<script>window.location.href='{Program.DefaultPath}?redirectTo={context.Request.Path}'</script>")
                            .ToString();
                    var routingTextBytes = Encoding.UTF8.GetBytes(routingText);
                    await context.Response.Body.WriteAsync(routingTextBytes,0,routingTextBytes.Length);
                    return;
                }
            });
            app.UseCors(CorsPolicy);

            app.UseSwaggerDocumenting();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.RegisterSwaggerService();
            services.ConfigureCustomServices();
            services.AddCors(opt => opt.AddPolicy(CorsPolicy, builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            }));
        }
    }
}