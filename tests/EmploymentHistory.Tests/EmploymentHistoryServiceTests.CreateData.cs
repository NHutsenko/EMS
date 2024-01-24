using System.Diagnostics.CodeAnalysis;
using EMS.EmploymentHistory.Tests.Mocks;
using FluentAssertions;
using Grpc.Core;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using EmploymentHistoryService = EMS.EmploymentHistory.Application.Services.EmploymentHistoryService;

namespace EMS.EmploymentHistory.Tests;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public partial class EmploymentHistoryServiceTests
{
    [Fact(DisplayName = "CreateEmployment should create employment record")]
    public async Task CreateData_TestCaseOne()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        NewEmploymentHistory request = new()
        {
            PersonId = _personClientMock.PersonResponse.Id,
            StartWork = new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
            PositionId = _positionClientMock.PositionResponse.Id,
            Employment = 100,
            ManagerId = _personClientMock.ManagerResponse.Id
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.CreateEmployment));

        NewStaff newStaffRequest = new()
        {
            Manager = request.ManagerId,
            Position = request.PositionId
        };
        NewHistory newHistoryRequest = new()
        {
            StaffId = _staffClientMock.StaffCreateResponse.Value,
            Data = new StaffHistory
            {
                Person = request.PersonId,
                Employment = request.Employment,
                CreatedOn = request.StartWork,
                Mentor = request.MentorId
            }
        };
        Int32Value personRequest = new()
        {
            Value = request.PersonId
        };
        Int32Value managerRequest = new()
        {
            Value = request.ManagerId
        };

        // Act
        Int32Value response = await _service.CreateEmployment(request, context);
        
        // Assert
        response.Value.Should().Be(_staffClientMock.StaffCreateResponse.Value);
        var personClientCalls = _personClientMock.PersonClient.ReceivedCalls();
        personClientCalls.ToArray()[0].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[0].GetArguments()[0].Should().Be(personRequest);
        personClientCalls.ToArray()[1].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[1].GetArguments()[0].Should().Be(managerRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());
        
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetByPerson(_staffClientMock.PersonStaffFoundRequest);
        var staffClientCalls = _staffClientMock.StaffClient.ReceivedCalls();
        
        var createStaffCall = staffClientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.CreateAsync));
        createStaffCall.Should().NotBe(null);
        createStaffCall!.GetArguments()[0].Should().Be(newStaffRequest);
        
        var createHistoryCall = staffClientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.CreateHistoryAsync));
        createHistoryCall.Should().NotBe(null);
        createHistoryCall!.GetArguments()[0].Should().Be(newHistoryRequest);
    }
    
    [Fact(DisplayName = "CreateEmployment should throw exception that position not found")]
    public async Task CreateData_TestCaseTwo()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        NewEmploymentHistory request = new()
        {
            PersonId = _personClientMock.PersonFoundRequest.Value,
            StartWork = new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
            PositionId = 999,
            Employment = 100,
            ManagerId = _personClientMock.ManagerResponse.Id
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.CreateEmployment));
        Int32Value personRequest = new()
        {
            Value = request.PersonId
        };
        Int32Value managerRequest = new()
        {
            Value = request.ManagerId
        };

        // Act
        NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateEmployment(request, context));
        
        // Assert
        exception.Should().NotBe(null);
        exception.Message.Should().Be($"Position with history id {request.PositionId} does not exists");
        
        var personClientCalls = _personClientMock.PersonClient.ReceivedCalls();
        personClientCalls.ToArray()[0].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[0].GetArguments()[0].Should().Be(personRequest);
        personClientCalls.ToArray()[1].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[1].GetArguments()[0].Should().Be(managerRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());
        
        _staffClientMock.StaffClient.Received(Quantity.None()).GetByPerson(Arg.Any<Int32Value>());
        _staffClientMock.StaffClient.ReceivedCalls().Count().Should().Be(0);
    }
    
    [Fact(DisplayName = "CreateEmployment should throw exception that record for date already exists")]
    public async Task CreateData_TestCaseThree()
    {
        // Arrange
        Timestamp wrongDate = _staffClientMock.PersonStaffHistoryResponse
            .OrderByDescending(e => e.History.CreatedOn.ToDateTime())
            .First()
            .History.CreatedOn.ToDateTime().AddMonths(-1)
            .ToTimestamp();
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        NewEmploymentHistory request = new()
        {
            PersonId = _personClientMock.PersonFoundRequest.Value,
            StartWork = wrongDate,
            PositionId = _positionClientMock.PositionResponse.Id,
            Employment = 100,
            ManagerId = _personClientMock.ManagerResponse.Id
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.CreateEmployment));
        Int32Value personRequest = new()
        {
            Value = request.PersonId
        };
        Int32Value managerRequest = new()
        {
            Value = request.ManagerId
        };

        // Act
        AlreadyExistsException exception = await Assert.ThrowsAsync<AlreadyExistsException>(() => _service.CreateEmployment(request, context));
        
        // Assert
        exception.Should().NotBe(null);
        var minDate = _staffClientMock.PersonStaffHistoryResponse
            .OrderByDescending(e => e.History.CreatedOn.ToDateTime())
            .First()
            .History.CreatedOn.ToDateTime().AddDays(1);
        exception.Message.Should().Be($"Employment period for date {wrongDate.ToDateTime().Date} already exists." +
                                            $"{Environment.NewLine}Minimum start work date is {minDate.Date}");
        
        var personClientCalls = _personClientMock.PersonClient.ReceivedCalls();
        personClientCalls.ToArray()[0].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[0].GetArguments()[0].Should().Be(personRequest);
        personClientCalls.ToArray()[1].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[1].GetArguments()[0].Should().Be(managerRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());
        
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetByPerson(_staffClientMock.PersonStaffFoundRequest);
    }
}