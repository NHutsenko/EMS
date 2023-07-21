using Grpc.AspNetCore.Server;
using Logging.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.Extensions;

public static class GrpcServiceCollectionExtensions
{
    public static void AddServiceLogging(this GrpcServiceOptions configuration)
    {
        configuration.Interceptors.Add<ServerLoggingInterceptor>();
    }

    public static IHttpClientBuilder AddClientLogging(this IHttpClientBuilder builder)
    {
        builder.AddInterceptor<ClientLoggingInterceptor>();
        return builder;
    }

}