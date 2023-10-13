using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using EMS.Protos;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace EMS.EmploymentHistory.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal sealed class PersonClientMock
{
    public PersonService.PersonServiceClient PersonClient { get; init; }
    public PersonData PersonResponse { get; init; }
    public PersonData ManagerResponse { get; init; }
    public PersonData MentorResponse { get; init; }
    public Int32Value PersonFoundRequest { get; init; }
    public Int32Value ManagerFoundRequest { get; init; }
    public Int32Value MentorFoundRequest { get; init; }
    public Int32Value PersonNotFoundRequest { get; init; }

    public PersonClientMock()
    {
        PersonClient = Substitute.For<PersonService.PersonServiceClient>(GrpcCoreMock.Channel);

        PersonResponse = new PersonData
        {
            Id = 1,
            General = new GeneralInfo
            {
                FirstName = "test",
                LastName = "test",
                BornOn = DateTime.UtcNow.ToTimestamp(),
                Login = "test",
                Gender = false,
                About = "test"
            },
            Comment = "test",
            Address = new AddressData
            {
                City = "test",
                Street = "test",
                Building = "test",
                House = "test"
            }
        };
        ManagerResponse = new PersonData
        {
            Id = 2,
            General = new GeneralInfo
            {
                FirstName = "test_manager",
                LastName = "test_manager",
                BornOn = DateTime.UtcNow.ToTimestamp(),
                Login = "test_manager",
                Gender = false,
                About = "test_manager"
            },
            Comment = "test_manager",
            Address = new AddressData
            {
                City = "test_manager",
                Street = "test_manager",
                Building = "test_manager",
                House = "test_manager"
            }
        };
        MentorResponse = new PersonData
        {
            Id = 3,
            General = new GeneralInfo
            {
                FirstName = "test_mentor",
                LastName = "test_mentor",
                BornOn = DateTime.UtcNow.ToTimestamp(),
                Login = "test_mentor",
                Gender = false,
                About = "test_mentor"
            },
            Comment = "test_mentor",
            Address = new AddressData
            {
                City = "test_mentor",
                Street = "test_mentor",
                Building = "test_mentor",
                House = "test_mentor"
            }
        };

        PersonFoundRequest = new Int32Value
        {
            Value = PersonResponse.Id
        };
        PersonClient.GetAsync(PersonFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(PersonResponse));
        
        ManagerFoundRequest = new Int32Value
        {
            Value = ManagerResponse.Id
        };
        PersonClient.GetAsync(ManagerFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(ManagerResponse));
        
        MentorFoundRequest = new Int32Value
        {
            Value = MentorResponse.Id
        };
        PersonClient.GetAsync(MentorFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(MentorResponse));
        
        PersonNotFoundRequest = new Int32Value
        {
            Value = 999
        };
        PersonClient.GetAsync(PersonNotFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Throws(new NotFoundException($"Person with id {PersonNotFoundRequest.Value} not found").ToRpcException());
        
        PersonClient.GetAsync(PersonNotFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(x => throw new NotFoundException($"Person with id {PersonNotFoundRequest.Value} not found").ToRpcException());

        PersonClient.GetAll(Arg.Any<Empty>(), Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetStreamResponse(new List<PersonData>()
            {
                PersonResponse,
                ManagerResponse,
                MentorResponse
            }));
    }
}