using System.Diagnostics.CodeAnalysis;
using EMS.EmploymentHistory.Tests.Mocks;
using EMS.Protos;
using FluentAssertions;
using Grpc.Core;
using NSubstitute;
using EmploymentHistoryService = EMS.EmploymentHistory.Services.EmploymentHistoryService;

namespace EMS.EmploymentHistory.Tests;

[ExcludeFromCodeCoverage]
public partial class EmploymentHistoryServiceTests
{
    private readonly PersonClientMock _personClientMock = new();
    private readonly StaffClientMock _staffClientMock = new();
    private readonly PositionClientMock _positionClientMock = new();
    private EmploymentHistoryService _service;
    
    [Fact(DisplayName = "GetPersonEmployment should return person employment records")]
    public async Task GetData_TestCaseOne()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        List<EmploymentHistoryData> response = _staffClientMock.PersonStaffHistoryResponse
            .Select(e => new EmploymentHistoryData
            {
                ManagerId = e.Manager,
                PositionId = e.Position,
                EmploymentHistoryId = e.Id,
                StartWork = e.History.CreatedOn,
                Employment = e.History.Employment,
                MentorId = e.History.Mentor
            })
            .ToList();
        IServerStreamWriter<EmploymentHistoryData> writer = GrpcCoreMock.GetTestServerStreamWriter<EmploymentHistoryData>();
        
        // Act
        await _service.GetPersonEmployment(_staffClientMock.PersonStaffFoundRequest, writer, GrpcCoreMock.GetCallContext(nameof(_service.GetPersonEmployment)));
        
        // Assert
        _staffClientMock.StaffClient.Received(1).GetByPerson(_staffClientMock.PersonStaffFoundRequest);
        foreach (var data in response)
        {
            await writer.Received(1).WriteAsync(data);
        }
    }
}