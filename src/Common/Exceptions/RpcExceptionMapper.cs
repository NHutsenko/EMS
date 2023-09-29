using System.Diagnostics.CodeAnalysis;
using EMS.Exceptions;
using Grpc.Core;

namespace Exceptions;

[ExcludeFromCodeCoverage]
public static class RpcExceptionMapper
{
    public static RpcException ToRpcException(this Exception ex)
    {
        return ex switch
        {
            not null when ex is AlreadyExistsException => new RpcException(new Status(StatusCode.AlreadyExists, ex.Message)),
            not null when ex is NotFoundException => new RpcException(new Status(StatusCode.NotFound, ex.Message)),
            not null when ex is BadRequestException => new RpcException(new Status(StatusCode.InvalidArgument, ex.Message)),
            _ => new RpcException(new Status(StatusCode.Internal, ex.Message))
        };
    }
}