using System.Diagnostics.CodeAnalysis;
using EMS.Protos;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using NSubstitute;

namespace EMS.EmploymentHistory.Tests;

[ExcludeFromCodeCoverage]
public class EmploymentHistoryServiceTests
{
    private readonly PersonService.PersonServiceClient _personClient;
    private PersonData _personResponse;

    public EmploymentHistoryServiceTests()
    {
        // Create Mock
        GrpcChannel channel = GrpcChannel.ForAddress("http://test.loc");
        _personClient = Substitute.For<PersonService.PersonServiceClient>(channel);

        _personResponse = new PersonData
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

        Int32Value correctRequest = Arg.Is<Int32Value>(e => e.Value == _personResponse.Id);
        _personClient.GetAsync(correctRequest, Arg.Any<Metadata>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(GetResponse(_personResponse));
    }

    [Fact(DisplayName = "Test")]
    public async Task TestCaseOne()
    {
        var data = await _personClient.GetAsync(new Int32Value
        {
            Value = _personResponse.Id
        }, new Metadata(), new DateTime(), CancellationToken.None);

        data.Should().Be(_personResponse);
    }

    private AsyncUnaryCall<T> GetResponse<T>(T responseData) where T : class
    {
        return new AsyncUnaryCall<T>(Task.FromResult(responseData), null, null, null, null);
    }
}