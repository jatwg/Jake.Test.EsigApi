using System.Reflection;
using Jake.Test.EsigApi.API;
using Jake.Test.EsigApi.API.Extensions;
using NLog;
using NLog.Web;

// Early init of NLog to allow startup logging
var logger = LogManager.Setup()
                      .LoadConfigurationFromFile()
                      .GetCurrentClassLogger();

try
{
    logger.Info("Starting up application");
    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Test database logging
    logger.Error("Test database logging entry - Please check if this appears in the database");

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Register application services
    builder.Services.AddApplicationServices(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseApplicationConfiguration(app.Environment);
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application stopped due to exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
} 