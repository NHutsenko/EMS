using System.Diagnostics.CodeAnalysis;
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
    public Int32Value PersonFoundRequest { get; init; }
    public Int32Value ManagerFoundRequest { get; init; }
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
        
        PersonNotFoundRequest = new Int32Value
        {
            Value = 999
        };
        PersonClient.GetAsync(PersonNotFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Throws(new NotFoundException($"Person with id {PersonNotFoundRequest.Value} not found").ToRpcException());
    }
}