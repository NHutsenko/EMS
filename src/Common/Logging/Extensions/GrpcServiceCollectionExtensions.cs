using EMS.Logging.Interceptors;
using Grpc.AspNetCore.Server;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Logging.Extensions;

public static class GrpcServiceCollectionExtensions
{
    public static void AddServiceLogging(this GrpcServiceOptions configuration)
    {
        configuration.Interceptors.Add<SServiceLoggingInterceptor>();
    }

    public static IHttpClientBuilder AddClientLogging(this IHttpClientBuilder builder)
    {
        builder.AddInterceptor<ClientLoggingInterceptor>();
        return builder;
    }

}