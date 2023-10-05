using System.Diagnostics.CodeAnalysis;
using EMS.EmploymentHistory.Tests.Mocks;
using EMS.Protos;
using FluentAssertions;
using Grpc.Core;
using Grpc.Core.Utils;
using NSubstitute;
using EmploymentHistoryService = EMS.EmploymentHistory.Services.EmploymentHistoryService;

namespace EMS.EmploymentHistory.Tests;

[ExcludeFromCodeCoverage]
public class EmploymentHistoryServiceTests
{
    private readonly PersonClientMock _personClientMock = new();
    private readonly StaffClientMock _staffClientMock = new();
    private readonly PositionClientMock _positionClientMock = new();
    private EmploymentHistoryService _service;
    
    [Fact(DisplayName = "GetPersonEmployment should return person employment records")]
    public async Task TestCaseOne()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        List<EmploymentHistoryData> response = _staffClientMock.PersonStaffHistoryResponse
            .Select(e => new EmploymentHistoryData
            {
                ManagerId = e.Manager,
                PositionId = e.Position,
                EmploymentId = e.Id,
                StartWork = e.History.CreatedOn,
                Employment = e.History.Employment
            })
            .ToList();
        IServerStreamWriter<EmploymentHistoryData> writer = GrpcCoreMock.GetTestServerStreamWriter<EmploymentHistoryData>();
        
        // Act
        await _service.GetPersonEmployment(_staffClientMock.PersonStaffFoundRequest, writer, GrpcCoreMock.GetCallContext(nameof(_service.GetPersonEmployment)));
        
        // Assert
        foreach (var data in response)
        {
            await writer.Received(1).WriteAsync(data);
        }
    }
    
    [Fact(DisplayName = "GetPersonEmployment should throw exception that employment not found for requested person")]
    public async Task TestCaseWto()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        IServerStreamWriter<EmploymentHistoryData> writer = GrpcCoreMock.GetTestServerStreamWriter<EmploymentHistoryData>();
        
       // Act
       RpcException? exception = await Assert.ThrowsAsync<RpcException>(() =>
           _service.GetPersonEmployment(_staffClientMock.PersonStaffNotFoundRequest, writer, GrpcCoreMock.GetCallContext(nameof(_service.GetPersonEmployment))));
       
       // Assert
       exception.Should().NotBe(null);
       exception.Status.StatusCode.Should().Be(StatusCode.NotFound);
       exception.Status.Detail.Should().Be($"Staff for person with id {_staffClientMock.PersonStaffNotFoundRequest.Value} not found");
    }
}