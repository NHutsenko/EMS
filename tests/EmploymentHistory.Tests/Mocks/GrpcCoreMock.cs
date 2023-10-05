using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Grpc.Core.Testing;
using Grpc.Core.Utils;
using Grpc.Net.Client;
using NSubstitute;

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

    public static AsyncServerStreamingCall<T> GetStreamResponse<T>(IEnumerable<T> response) where T : class
    {
        AsyncServerStreamingCall<T>? mock = Substitute.For<AsyncServerStreamingCall<T>>();
        IAsyncStreamReader<T> reader = new MyAsyncStreamReader<T>(response);
        mock.ResponseStream.Returns(reader);
        return mock;
    }
    
    private sealed class MyAsyncStreamReader<T> : IAsyncStreamReader<T>
    {
        private readonly IEnumerator<T> enumerator;

        public MyAsyncStreamReader(IEnumerable<T> results)
        {
            enumerator = results.GetEnumerator();
        }

        public T Current => enumerator.Current;

        public Task<bool> MoveNext(CancellationToken cancellationToken) =>
            Task.Run(() => enumerator.MoveNext());
    }
}