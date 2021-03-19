using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Moq;
using static EMS.Common.Protos.Teams;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class TeamsClientMock: BaseMock
    {
        public static Mock<TeamsClient> SetupMock()
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://test.loc");
            Mock<TeamsClient> mock = new Mock<TeamsClient>(channel);

            mock.Setup(m => m.AddAsync(It.IsAny<TeamData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<TeamData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
            {
                ThrowExceptionIfNeeded();
                return Response as BaseResponse;
            });

            mock.Setup(m => m.UpdateAsync(It.IsAny<TeamData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<TeamData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
            {
                ThrowExceptionIfNeeded();
                return Response as BaseResponse;
            });

            mock.Setup(m => m.DeleteAsync(It.IsAny<TeamData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<TeamData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
            {
                ThrowExceptionIfNeeded();
                return Response as BaseResponse;
            });

            mock.Setup(m => m.GetAll(It.IsAny<Empty>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<Empty, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
            {
                ThrowExceptionIfNeeded();
                return Response as TeamsResponse;
            });

            mock.Setup(m => m.GetById(It.IsAny<TeamRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<TeamRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
            {
                ThrowExceptionIfNeeded();
                return Response as TeamResponse;
            });

            return mock;
        }
    }
}
