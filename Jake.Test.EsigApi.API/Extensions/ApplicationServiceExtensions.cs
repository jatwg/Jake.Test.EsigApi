namespace Jake.Test.EsigApi.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfiguration(configuration);

        // Add other application services here
        // Example: services.AddScoped<IMyService, MyService>();

        return services;
    }

    public static IApplicationBuilder UseApplicationConfiguration(
        this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwaggerConfiguration();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        return app;
    }
} 