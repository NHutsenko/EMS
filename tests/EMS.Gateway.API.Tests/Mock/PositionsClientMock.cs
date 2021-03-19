using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Moq;
using static EMS.Common.Protos.Positions;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class PositionsClientMock: BaseMock
    {
        public static Mock<PositionsClient> SetupMock()
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://test.loc");
            Mock<PositionsClient> mock = new Mock<PositionsClient>(channel);

            mock.Setup(m => m.AddAsync(It.IsAny<PositionData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<PositionData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });

            mock.Setup(m => m.UpdateAsync(It.IsAny<PositionData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<PositionData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });

            mock.Setup(m => m.DeleteAsync(It.IsAny<PositionData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<PositionData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });

            mock.Setup(m => m.GetAll(It.IsAny<Empty>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<Empty, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as PositionsResponse;
                });

            mock.Setup(m => m.GetById(It.IsAny<PositionRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<PositionRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as PositionResponse;
                });

            return mock;
        }
    }
}
