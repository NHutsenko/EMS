using EMS.Logging.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Logging.Extensions;

public static class LoggerServiceCollectionExtensions
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ClientLoggingInterceptor>();
        return services;
    }

}