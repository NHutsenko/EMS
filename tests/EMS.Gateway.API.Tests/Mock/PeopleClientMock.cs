using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Moq;
using static EMS.Common.Protos.People;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class PeopleClientMock: BaseMock
    {
        public static Mock<PeopleClient> SetupMock()
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://test.loc");
            Mock<PeopleClient> mock = new(channel);

            mock.Setup(m => m.AddAsync(It.IsAny<PersonData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
               .Returns<PersonData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
               {
                   ThrowExceptionIfNeeded();
                   return Response as BaseResponse;
               });


            mock.Setup(m => m.AddContactAsync(It.IsAny<ContactData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
               .Returns<ContactData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
               {
                   ThrowExceptionIfNeeded();
                   return Response as BaseResponse;
               });

            mock.Setup(m => m.AddPhotoAsync(It.IsAny<PhotoData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
              .Returns<PhotoData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
              {
                  ThrowExceptionIfNeeded();
                  return Response as BaseResponse;
              });

            mock.Setup(m => m.AddAsync(It.IsAny<PersonData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
               .Returns<PersonData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
               {
                   ThrowExceptionIfNeeded();
                   return Response as BaseResponse;
               });

            mock.Setup(m => m.UpdateAsync(It.IsAny<PersonData>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<PersonData, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as BaseResponse;
                });

            mock.Setup(m => m.GetById(It.IsAny<ByPersonIdRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<ByPersonIdRequest, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as PersonResponse;
                });

            mock.Setup(m => m.GetAll(It.IsAny<Empty>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns<Empty, Metadata, DateTime?, CancellationToken>((request, metdata, timestamp, token) =>
                {
                    ThrowExceptionIfNeeded();
                    return Response as PeopleResponse;
                });

            return mock;
        }
    }
}
