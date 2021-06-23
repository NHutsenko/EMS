using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Moq;
using static EMS.Common.Protos.RoadMaps;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class RoadMapsClientMock: BaseMock
    {
        public static Mock<RoadMapsClient> SetupMock()
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://test.loc");
            Mock<RoadMapsClient> mock = new Mock<RoadMapsClient>(channel);

            mock.Setup(m => m.AddAsync(It.IsAny<RoadMapData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<RoadMapData, Metadata, DateTime, CancellationToken>((roadMap, metadata, dateTime, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });
            mock.Setup(m => m.UpdateAsync(It.IsAny<RoadMapData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<RoadMapData, Metadata, DateTime, CancellationToken>((roadMap, metadata, dateTime, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });
            mock.Setup(m => m.DeleteAsync(It.IsAny<RoadMapData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<RoadMapData, Metadata, DateTime, CancellationToken>((roadMap, metadata, dateTime, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });
            mock.Setup(m => m.GetByStaffId(It.IsAny<ByStaffRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<ByStaffRequest, Metadata, DateTime, CancellationToken>((roadMap, metadata, dateTime, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as RoadMapResponse;
                });

            return mock;
        }
    }
}
