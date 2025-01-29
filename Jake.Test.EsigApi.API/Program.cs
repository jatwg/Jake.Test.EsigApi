using System.Reflection;
using Jake.Test.EsigApi.API;
using Jake.Test.EsigApi.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

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