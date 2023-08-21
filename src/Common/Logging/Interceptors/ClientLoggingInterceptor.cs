using System.Text.Json;
using EMS.Logging.Constants;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace EMS.Logging.Interceptors;

public sealed class ClientLoggingInterceptor : Interceptor
{
    private readonly ILogger<ClientLoggingInterceptor> _logger;
    private readonly IMemoryCache _memoryCache;

    public ClientLoggingInterceptor(ILoggerFactory loggerFactory, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _logger = loggerFactory.CreateLogger<ClientLoggingInterceptor>();
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Guid scopeId = GetScopeId();

        Metadata headers = new()
        {
            { LogConstants.ScopeId, scopeId.ToString() }
        };
        CallOptions newOptions = context.Options.WithHeaders(headers);
        ClientInterceptorContext<TRequest, TResponse> newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            newOptions);

        _logger.LogInformation("Starting call. Method: {methodName}.{newLine} Message {body}", context.Method.Name, Environment.NewLine, JsonSerializer.Serialize(request));
        return continuation(request, newContext);
    }

    private Guid GetScopeId()
    {
        object? scopeIdValue = _memoryCache.Get(LogConstants.ScopeId);
        bool parsed = Guid.TryParse((scopeIdValue ?? String.Empty).ToString(), out Guid scopeId);
        return !parsed ? Guid.NewGuid() : scopeId;
    }
}
