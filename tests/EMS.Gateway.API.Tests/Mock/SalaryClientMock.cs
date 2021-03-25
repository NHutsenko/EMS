using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Moq;
using static EMS.Common.Protos.Salary;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class SalaryClientMock: BaseMock
    {
        public static Mock<SalaryClient> SetupMock()
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://test.loc");
            Mock<SalaryClient> mock = new(channel);

            mock.Setup(m => m.GetSalary(It.IsAny<SalaryRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
               .Returns<SalaryRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
               {
                   ThrowExceptionIfNeeded();
                   return Response as ISalaryResponse;
               });

            return mock;
        }
    }
}
