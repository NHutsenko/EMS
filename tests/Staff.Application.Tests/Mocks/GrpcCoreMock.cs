using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Grpc.Core.Testing;
using Grpc.Core.Utils;
using Grpc.Net.Client;
using NSubstitute;

namespace EMS.Staff.Application.Tests.Mocks;

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
        return new AsyncUnaryCall<T>(Task.FromResult(responseData),
            Task.FromResult(new Metadata()),
            () => new Status(StatusCode.OK, string.Empty),
            () => new Metadata(),
            () => { });
    }

    public static AsyncServerStreamingCall<T> GetStreamResponse<T>(IEnumerable<T> response) where T : class
    {
        IAsyncStreamReader<T> streamReader = new TestAsyncStreamReader<T>(response);
        AsyncServerStreamingCall<T>? mock = new AsyncServerStreamingCall<T>(streamReader,
            Task.FromResult(new Metadata()),
            () => new Status(StatusCode.OK, string.Empty),
            () => new Metadata(),
            () => { });
        return mock;
    }

    public static IServerStreamWriter<T> GetTestServerStreamWriter<T>()
    {
        IServerStreamWriter<T> writer = Substitute.For<IServerStreamWriter<T>>();

        writer.WriteAsync(Arg.Any<T>()).Returns(Task.CompletedTask);
        return writer;
    }

    private sealed class TestAsyncStreamReader<T> : IAsyncStreamReader<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public TestAsyncStreamReader(IEnumerable<T> results)
        {
            _enumerator = results.GetEnumerator();
        }

        public T Current => _enumerator.Current;

        public Task<bool> MoveNext(CancellationToken cancellationToken) =>
            Task.Run(() => _enumerator.MoveNext());
    }
}