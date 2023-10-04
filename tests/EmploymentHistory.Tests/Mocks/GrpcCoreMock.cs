using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Grpc.Core.Testing;
using Grpc.Core.Utils;
using Grpc.Net.Client;

namespace EMS.EmploymentHistory.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal static class GrpcCoreMock
{
    public static GrpcChannel Channel { get; } = GrpcChannel.ForAddress("http://test.loc");
    public static ServerCallContext GetCallContext(string methodName)
    {
        return TestServerCallContext.Create(methodName,
            null,
            DateTime.UtcNow.AddHours(1),
            new Metadata(),
            CancellationToken.None,
            "127.0.0.1",
            null,
            null,
            _ => TaskUtils.CompletedTask,
            () => new WriteOptions(), _ => { }
        );
    }
    
    public static AsyncUnaryCall<T> GetAsyncUnaryCallResponse<T>(T responseData) where T : class
    {
        return new AsyncUnaryCall<T>(Task.FromResult(responseData), null, null, null, null);
    }
}