using System.Diagnostics.CodeAnalysis;
using Grpc.Core;

namespace Exceptions;

[ExcludeFromCodeCoverage]
public static class RpcExceptionMapper
{
    public static RpcException ToRpcException(this Exception ex)
    {
        return ex switch
        {
            not null when ex.GetType() == typeof(AlreadyExistsException) => new RpcException(new Status(StatusCode.AlreadyExists, ex.Message)),
            not null when ex.GetType() == typeof(NotFoundException) => new RpcException(new Status(StatusCode.NotFound, ex.Message)),
            _ => new RpcException(new Status(StatusCode.Internal, ex.Message))
        };
    }
}