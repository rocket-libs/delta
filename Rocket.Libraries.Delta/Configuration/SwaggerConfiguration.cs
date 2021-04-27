using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Rocket.Libraries.Delta.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerConfiguration
    {
        private const string APITitle = "Delta API";

        private const string APIVersion = "v1";

        private static string AuthenticatedNotice => GetListItem(string.Empty);

        private static string OptionalNotice => GetListItem("All parameters are by default mandatory, unless explictly marked as optional.");

        public static void UseSwaggerDocumenting(this IApplicationBuilder app)
        {
            SwaggerBuilderExtensions.UseSwagger(app);
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{APITitle} {APIVersion}");
            });
        }

        public static void RegisterSwaggerService(this IServiceCollection services)
        {
            var notices = $"<span style='color:#b0178a; margin-top:20px'><i><b>Important:</b> Unless explicitly stated otherwise: <ol>{(AuthenticatedNotice)}{OptionalNotice}</ol></i></span>";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(APIVersion, new OpenApiInfo
                {
                    Title = APITitle,
                    Version = APIVersion.ToUpper(CultureInfo.InvariantCulture),
                    Description = $"<div style='font-size:16px;'>Documentation of endpoints available in the {APITitle}.<br>{notices}</div>",
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private static string GetListItem(string item)
        {
            return $"<li>{item}</li>";
        }
    }
}