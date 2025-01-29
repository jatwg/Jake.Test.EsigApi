using System.Reflection;
using Jake.Test.EsigApi.API.Configuration;
using Microsoft.OpenApi.Models;

namespace Jake.Test.EsigApi.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SwaggerOptions>(
            configuration.GetSection(SwaggerOptions.SectionName));

        services.AddSwaggerGen(c =>
        {
            var options = new SwaggerOptions();
            configuration.GetSection(SwaggerOptions.SectionName).Bind(options);

            c.SwaggerDoc(options.Version, new OpenApiInfo
            {
                Title = options.Title,
                Version = options.Version,
                Description = options.Description,
                Contact = new OpenApiContact
                {
                    Name = options.Contact.Name,
                    Email = options.Contact.Email
                },
                License = new OpenApiLicense
                {
                    Name = options.License.Name,
                    Url = new Uri(options.License.Url)
                }
            });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            // Use full schema names to prevent conflicts
            c.CustomSchemaIds(type => type.FullName);
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(
        this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Signature API v1");
            c.RoutePrefix = "swagger";
            c.DocumentTitle = "E-Signature API Documentation";
            c.DefaultModelsExpandDepth(2);
            c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
            c.EnableDeepLinking();
            c.DisplayRequestDuration();
        });

        return app;
    }
} 