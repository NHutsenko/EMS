using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Moq;
using static EMS.Common.Protos.OtherPayments;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class OtherPaymentsClientMock: BaseMock
    {
        public static Mock<OtherPaymentsClient> SetupMock()
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://test.loc");
            Mock<OtherPaymentsClient> mock = new(channel);

            mock.Setup(m => m.AddAsync(It.IsAny<OtherPaymentData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
               .Returns<OtherPaymentData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
               {
                   ThrowExceptionIfNeeded();
                   return Response as BaseResponse;
               });

            mock.Setup(m => m.UpdateAsync(It.IsAny<OtherPaymentData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<OtherPaymentData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });

            mock.Setup(m => m.DeleteAsync(It.IsAny<OtherPaymentData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<OtherPaymentData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });

            mock.Setup(m => m.GetByPersonId(It.IsAny<ByPersonIdRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<ByPersonIdRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as OtherPaymentsResponse;
                });

            mock.Setup(m => m.GetByPersonIdAndDateRange(It.IsAny<ByPersonIdAndDateRangeRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<ByPersonIdAndDateRangeRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as OtherPaymentsResponse;
                });

            return mock;
        }
    }
}
