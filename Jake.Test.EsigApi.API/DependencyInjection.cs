using Jake.Test.EsigApi.Application.Interfaces;
using Jake.Test.EsigApi.Application.Services;
using Jake.Test.EsigApi.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Jake.Test.EsigApi.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IESignatureRepository, InMemoryESignatureRepository>();
        services.AddScoped<IESignatureService, ESignatureService>();

        return services;
    }
} 