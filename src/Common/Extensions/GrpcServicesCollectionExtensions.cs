using System.Diagnostics.CodeAnalysis;
using EMS.Logging.Extensions;
using Grpc.Core;
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
    
    public static async Task<IEnumerable<T>> ToEnumerableAsync<T>(this AsyncServerStreamingCall<T> call, CancellationToken cancellationToken) where T: class
    {
        IAsyncEnumerable<T> responseData = call.ResponseStream.ReadAllAsync(cancellationToken);
        IEnumerable<T> data = new List<T>();
        await foreach (T item in responseData)
        {
            _ = data.Append(item);
        }

        return data;
    }

    public static async Task WriteResponseAsync<T>(this IServerStreamWriter<T> stream, IEnumerable<T> data, CancellationToken cancellationToken) where T : class
    {
        foreach (var item in data)
        {
            await stream.WriteAsync(item, cancellationToken);
        }
    }
}