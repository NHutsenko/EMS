using System.Diagnostics.CodeAnalysis;
using EMS.EmploymentHistory.Tests.Mocks;
using EMS.Exceptions;
using EMS.Protos;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using EmploymentHistoryService = EMS.EmploymentHistory.Services.EmploymentHistoryService;

namespace EMS.EmploymentHistory.Tests;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public partial class EmploymentHistoryServiceTests
{
    [Fact(DisplayName = "UpdateEmployment should throws exception that position to set not found")]
    public async Task EditData_CaseOne()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        EmploymentHistoryData request = new()
        {
            EmploymentHistoryId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = 999,
            ManagerId = _personClientMock.ManagerFoundRequest.Value
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        // Act
        NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateEmployment(request, context));

        // Assert
        exception.Message.Should().Be($"Position with history id {request.PositionId} does not exists");

        var staffCall = _staffClientMock.StaffClient.ReceivedCalls();
        var getByHistoryIdCall = staffCall.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.GetByHistoryIdAsync));
        getByHistoryIdCall.Should().NotBe(null);
        getByHistoryIdCall!.GetArguments()[0].Should().Be(_staffClientMock.StaffFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.None()).GetAll(Arg.Any<Empty>());
    }

    [Fact(DisplayName = "UpdateEmployment should throws exception that start work should be greater than last record plus one day")]
    public async Task EditData_CaseTwo()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        Timestamp dateToSet = _staffClientMock.PersonStaffHistoryResponse
            .OrderByDescending(e => e.History.CreatedOn)
            .First()
            .History.CreatedOn
            .ToDateTime()
            .AddDays(-5)
            .ToTimestamp();
        EmploymentHistoryData request = new()
        {
            EmploymentHistoryId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _positionClientMock.PositionResponse.Id,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = dateToSet
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        // Act
        AlreadyExistsException exception = await Assert.ThrowsAsync<AlreadyExistsException>(() => _service.UpdateEmployment(request, context));

        // Assert
        DateTime minValid = _staffClientMock.PersonStaffHistoryResponse
            .OrderByDescending(e => e.History.CreatedOn)
            .First()
            .History.CreatedOn
            .ToDateTime()
            .AddDays(1);
        exception.Message.Should().Be($"Employment period for date {dateToSet.ToDateTime().Date} already exists." +
                                      $"{Environment.NewLine}Minimum start work date is {minValid.Date}");

        var staffCall = _staffClientMock.StaffClient.ReceivedCalls();
        var getByHistoryIdCall = staffCall.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.GetByHistoryIdAsync));
        getByHistoryIdCall.Should().NotBe(null);
        getByHistoryIdCall!.GetArguments()[0].Should().Be(_staffClientMock.StaffFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());
    }

    [Fact(DisplayName = "UpdateEmployment should call set date")]
    public async Task EditData_CaseThree()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        Timestamp dateToSet = _staffClientMock.StaffSecond.History.CreatedOn
            .ToDateTime()
            .AddMonths(1)
            .ToTimestamp();
        EmploymentHistoryData request = new()
        {
            EmploymentHistoryId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _staffClientMock.StaffSecond.Position,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = dateToSet,
            Employment = _staffClientMock.StaffSecond.History.Employment
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewDate expectedSetDateRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Date = dateToSet
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        var staffCall = _staffClientMock.StaffClient.ReceivedCalls();
        var getByHistoryIdCall = staffCall.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.GetByHistoryIdAsync));
        getByHistoryIdCall.Should().NotBe(null);
        getByHistoryIdCall!.GetArguments()[0].Should().Be(_staffClientMock.StaffFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        var calls = _staffClientMock.StaffClient.ReceivedCalls();
        var setCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetDateAsync));
        setCall.Should().NotBe(null);
        setCall!.GetArguments()[0].Should().Be(expectedSetDateRequest);

        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetEmploymentAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetManagerAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetPositionAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetMentorAsync)).Should().Be(false);
    }

    [Fact(DisplayName = "UpdateEmployment should call set position")]
    public async Task EditData_CaseFour()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        EmploymentHistoryData request = new()
        {
            EmploymentHistoryId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _positionClientMock.PositionResponse.Id,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = _staffClientMock.StaffSecond.History.CreatedOn,
            Employment = _staffClientMock.StaffSecond.History.Employment
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewPosition expectedPositionRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Position = request.PositionId
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        var staffCall = _staffClientMock.StaffClient.ReceivedCalls();
        var getByHistoryIdCall = staffCall.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.GetByHistoryIdAsync));
        getByHistoryIdCall.Should().NotBe(null);
        getByHistoryIdCall!.GetArguments()[0].Should().Be(_staffClientMock.StaffFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        var calls = _staffClientMock.StaffClient.ReceivedCalls();
        var setCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetPositionAsync));
        setCall.Should().NotBe(null);
        setCall!.GetArguments()[0].Should().Be(expectedPositionRequest);
        
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetEmploymentAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetManagerAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetDateAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetMentorAsync)).Should().Be(false);
    }
    
    [Fact(DisplayName = "UpdateEmployment should call set employment")]
    public async Task EditData_CaseEight()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        EmploymentHistoryData request = new()
        {
            EmploymentHistoryId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _staffClientMock.StaffSecond.Position,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = _staffClientMock.StaffSecond.History.CreatedOn,
            Employment = 80
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewEmployment expectedEmploymentRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Employment = request.Employment
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        var staffCall = _staffClientMock.StaffClient.ReceivedCalls();
        var getByHistoryIdCall = staffCall.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.GetByHistoryIdAsync));
        getByHistoryIdCall.Should().NotBe(null);
        getByHistoryIdCall!.GetArguments()[0].Should().Be(_staffClientMock.StaffFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        var calls = _staffClientMock.StaffClient.ReceivedCalls();
        var setCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetEmploymentAsync));
        setCall.Should().NotBe(null);
        setCall!.GetArguments()[0].Should().Be(expectedEmploymentRequest);
        
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetDateAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetManagerAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetPositionAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetMentorAsync)).Should().Be(false);
    }
    
    [Fact(DisplayName = "UpdateEmployment should call set manager")]
    public async Task EditData_CaseNine()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        EmploymentHistoryData request = new()
        {
            EmploymentHistoryId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _staffClientMock.StaffSecond.Position,
            ManagerId = _personClientMock.MentorFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = _staffClientMock.StaffSecond.History.CreatedOn,
            Employment = _staffClientMock.StaffSecond.History.Employment
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewManager expectedManagerRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Manager = _personClientMock.MentorFoundRequest.Value,
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        var staffCall = _staffClientMock.StaffClient.ReceivedCalls();
        var getByHistoryIdCall = staffCall.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.GetByHistoryIdAsync));
        getByHistoryIdCall.Should().NotBe(null);
        getByHistoryIdCall!.GetArguments()[0].Should().Be(_staffClientMock.StaffFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());
        
        var calls = _staffClientMock.StaffClient.ReceivedCalls();
        var setCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetManagerAsync));
        setCall.Should().NotBe(null);
        setCall!.GetArguments()[0].Should().Be(expectedManagerRequest);

        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetDateAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetEmploymentAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetPositionAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetMentorAsync)).Should().Be(false);
    }
    
    [Fact(DisplayName = "UpdateEmployment should call set mentor")]
    public async Task EditData_CaseTen()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        EmploymentHistoryData request = new()
        {
            EmploymentHistoryId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _staffClientMock.StaffSecond.Position,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.ManagerFoundRequest.Value,
            StartWork = _staffClientMock.StaffSecond.History.CreatedOn,
            Employment = _staffClientMock.StaffSecond.History.Employment
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewMentor expectedMentorRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Mentor = _personClientMock.ManagerFoundRequest.Value,
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        var staffCall = _staffClientMock.StaffClient.ReceivedCalls();
        var getByHistoryIdCall = staffCall.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.GetByHistoryIdAsync));
        getByHistoryIdCall.Should().NotBe(null);
        getByHistoryIdCall!.GetArguments()[0].Should().Be(_staffClientMock.StaffFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        var calls = _staffClientMock.StaffClient.ReceivedCalls();
        var setCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetMentorAsync));
        setCall.Should().NotBe(null);
        setCall!.GetArguments()[0].Should().Be(expectedMentorRequest);
        
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetDateAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetPositionAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetEmploymentAsync)).Should().Be(false);
        calls.Any(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetManagerAsync)).Should().Be(false);
    }
    
    [Fact(DisplayName = "UpdateEmployment should update all fields")]
    public async Task EditData_CaseEleven()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        Timestamp dateToSet = _staffClientMock.StaffSecond.History.CreatedOn
            .ToDateTime()
            .AddMonths(1)
            .ToTimestamp();
        EmploymentHistoryData request = new()
        {
            EmploymentHistoryId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _positionClientMock.PositionResponse.Id,
            ManagerId = _personClientMock.MentorFoundRequest.Value,
            MentorId = _personClientMock.ManagerFoundRequest.Value,
            StartWork = dateToSet,
            Employment = 80
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewMentor expectedMentorRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Mentor = request.MentorId,
        };
        NewManager expectedManagerRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Manager = request.ManagerId,
        };
        NewEmployment expectedEmploymentRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Employment = request.Employment
        };
        NewDate expectedDateRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Date = dateToSet
        };
        NewPosition expectedPositionRequest = new()
        {
            StaffId = request.EmploymentHistoryId,
            Position = request.PositionId
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        var staffCall = _staffClientMock.StaffClient.ReceivedCalls();
        var getByHistoryIdCall = staffCall.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.GetByHistoryIdAsync));
        getByHistoryIdCall.Should().NotBe(null);
        getByHistoryIdCall!.GetArguments()[0].Should().Be(_staffClientMock.StaffFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());
        
        var calls = _staffClientMock.StaffClient.ReceivedCalls();
        var setDateCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetDateAsync));
        setDateCall.Should().NotBe(null);
        setDateCall!.GetArguments()[0].Should().Be(expectedDateRequest);
        
        var setPositionCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetPositionAsync));
        setPositionCall.Should().NotBe(null);
        setPositionCall!.GetArguments()[0].Should().Be(expectedPositionRequest);
        
        var setEmploymentlCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetEmploymentAsync));
        setEmploymentlCall.Should().NotBe(null);
        setEmploymentlCall!.GetArguments()[0].Should().Be(expectedEmploymentRequest);
        
        var setManagerlCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetManagerAsync));
        setManagerlCall.Should().NotBe(null);
        setManagerlCall!.GetArguments()[0].Should().Be(expectedManagerRequest);
        
        var setMentorlCall = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_staffClientMock.StaffClient.SetMentorAsync));
        setMentorlCall.Should().NotBe(null);
        setMentorlCall!.GetArguments()[0].Should().Be(expectedMentorRequest);
    }
}