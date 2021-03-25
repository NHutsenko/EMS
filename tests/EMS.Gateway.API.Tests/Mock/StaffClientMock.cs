using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Moq;
using static EMS.Common.Protos.Staffs;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class StaffClientMock: BaseMock
    {
        public static Mock<StaffsClient> SetupMock()
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://test.loc");
            Mock<StaffsClient> mock = new(channel);

            mock.Setup(m => m.AddAsync(It.IsAny<StaffData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
               .Returns<StaffData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
               {
                   ThrowExceptionIfNeeded();
                   return Response as BaseResponse;
               });

            mock.Setup(m => m.UpdateAsync(It.IsAny<StaffData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<StaffData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });
            mock.Setup(m => m.DeleteAsync(It.IsAny<StaffData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<StaffData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });
            mock.Setup(m => m.GetAll(It.IsAny<Empty>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<Empty, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as StaffResponse;
                });
            mock.Setup(m => m.GetByPersonId(It.IsAny<ByPersonIdRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<ByPersonIdRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as StaffResponse;
                });
            mock.Setup(m => m.GetByManagerId(It.IsAny<ByPersonIdRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<ByPersonIdRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as StaffResponse;
                });

            return mock;
        }
    }
}
