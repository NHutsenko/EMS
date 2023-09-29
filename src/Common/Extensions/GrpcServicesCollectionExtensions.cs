using System.Diagnostics.CodeAnalysis;
using EMS.Logging.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Extensions;

[ExcludeFromCodeCoverage]
public static class GrpcServicesCollectionExtensions
{
    public static IServiceCollection AddGrpcServer(this IServiceCollection services)
    {
        services.AddGrpc(cfg =>
        {
            cfg.EnableDetailedErrors = true;
            cfg.AddServiceLogging();
            cfg.MaxReceiveMessageSize = null;
        });
        return services;
    }

    public static IHttpClientBuilder ConfigureClient(this IHttpClientBuilder builder)
    {
        builder.AddClientLogging();
        builder.ConfigureChannel(cfg => cfg.MaxReceiveMessageSize = null);
        return builder;
    }
}