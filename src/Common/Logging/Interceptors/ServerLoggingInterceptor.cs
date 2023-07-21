using System.Text.Json;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Logging.Constants;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Logging.Interceptors;

public sealed class ServerLoggingInterceptor : Interceptor
{
    private readonly ILogger<ServerLoggingInterceptor> _logger;
    private readonly IMemoryCache _memoryCache;

    public ServerLoggingInterceptor(ILoggerFactory loggerFactory, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _logger = loggerFactory.CreateLogger<ServerLoggingInterceptor>();
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Guid scopeId = GetScopeId(context);
        using (_logger.BeginScope(new[] { new KeyValuePair<string, object>(LogConstants.ScopeId, scopeId) }))
        {
            _logger.LogInformation("Starting call. Method: {methodName}.{newLine} Message {body}", context.Method, Environment.NewLine, JsonSerializer.Serialize(request));
            try
            {
                return await continuation(request, context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured while calling {method}", context.Method);
                throw;
            }
        }
    }

    private Guid GetScopeId(ServerCallContext context)
    {
        Metadata.Entry? scopeId = context.RequestHeaders.FirstOrDefault(e => e.Key.Equals(LogConstants.ScopeId, StringComparison.InvariantCultureIgnoreCase));
        Guid scopeIdValue = Guid.NewGuid();
        if (scopeId == null)
        {
            _memoryCache.Set(LogConstants.ScopeId, scopeIdValue);
            return scopeIdValue;
        }

        if (Guid.TryParse(scopeId.Value, out Guid scopeIdGuid)  is false)
        {
            _memoryCache.Set(LogConstants.ScopeId, scopeIdValue);
            return scopeIdValue;
        }
        _memoryCache.Set(LogConstants.ScopeId, scopeIdGuid);
        return scopeIdGuid;
    }
}
