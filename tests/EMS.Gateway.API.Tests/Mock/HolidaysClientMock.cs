using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Moq;
using static EMS.Common.Protos.Holidays;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class HolidaysClientMock: BaseMock
    {
        public static Mock<HolidaysClient> SetupMock()
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://test.loc");
            Mock<HolidaysClient> mock = new(channel);

            mock.Setup(m => m.AddAsync(It.IsAny<HolidayData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
               .Returns<HolidayData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
               {
                   ThrowExceptionIfNeeded();
                   return Response as BaseResponse;
               });

            mock.Setup(m => m.UpdateAsync(It.IsAny<HolidayData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<HolidayData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });

            mock.Setup(m => m.DeleteAsync(It.IsAny<HolidayData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<HolidayData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });

            mock.Setup(m => m.GetAll(It.IsAny<Empty>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<Empty, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as HolidaysResponse;
                });

            mock.Setup(m => m.GetByDateRange(It.IsAny<ByDateRangeRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<ByDateRangeRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as HolidaysResponse;
                });

            return mock;
        }
    }
}
