using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Grpc.Core.Testing;
using Grpc.Core.Utils;

namespace EMS.EmploymentHistory.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal static class CallContextMock
{
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
}