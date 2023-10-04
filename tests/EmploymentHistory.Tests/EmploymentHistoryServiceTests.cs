using System.Diagnostics.CodeAnalysis;
using EMS.EmploymentHistory.Tests.Mocks;
using FluentAssertions;
using Grpc.Core;

namespace EMS.EmploymentHistory.Tests;

[ExcludeFromCodeCoverage]
public class EmploymentHistoryServiceTests
{
    private readonly PersonClientMock _personClientMock = new();
    
    [Fact(DisplayName = "Test")]
    public async Task TestCaseOne()
    {
        var data = await _personClientMock.PersonClient.GetAsync(_personClientMock.PersonFoundRequest, new Metadata(), new DateTime(), CancellationToken.None);

        data.Should().Be(_personClientMock.PersonResponse);

        Assert.Throws<RpcException>(() => _personClientMock.PersonClient.GetAsync(_personClientMock.PersonNotFoundRequest, new Metadata(), new DateTime(), CancellationToken.None));
    }
}