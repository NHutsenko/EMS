using EMS.EmploymentHistory.Tests.Mocks;
using EMS.Protos;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace EMS.EmploymentHistory.Tests;

public partial class EmploymentHistoryServiceTests
{
    [Fact(DisplayName = "UpdateEmployment should throws exception that staff to edit nit found")]
    public async Task EditData_CaseOne()
    {
        // Arrange
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffNotFoundRequest.Value
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        // Act
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.UpdateEmployment(request, context));

        // Assert
        exception.Status.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Status.Detail.Should().Be($"Staff with id {request.EmploymentId} not found");

        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.None()).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.None()).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }

    [Fact(DisplayName = "UpdateEmployment should throws exception that position to set not found")]
    public async Task EditData_CaseTwo()
    {
        // Arrange
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = 999
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        // Act
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.UpdateEmployment(request, context));

        // Assert
        exception.Status.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Status.Detail.Should().Be($"Position with id {request.PositionId} does not exists");

        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.None()).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }

    [Fact(DisplayName = "UpdateEmployment should throws exception that manager to set not found")]
    public async Task EditData_CaseThree()
    {
        // Arrange
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _positionClientMock.PositionResponse.Id,
            ManagerId = _personClientMock.PersonNotFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        // Act
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.UpdateEmployment(request, context));

        // Assert
        exception.Status.StatusCode.Should().Be(StatusCode.NotFound);

        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }

    [Fact(DisplayName = "UpdateEmployment should throws exception that mentor to set not found")]
    public async Task EditData_CaseFour()
    {
        // Arrange
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _positionClientMock.PositionResponse.Id,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.PersonNotFoundRequest.Value
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        // Act
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.UpdateEmployment(request, context));

        // Assert
        exception.Status.StatusCode.Should().Be(StatusCode.NotFound);

        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }

    [Fact(DisplayName = "UpdateEmployment should throws exception that start work should be greater than last record plus one day")]
    public async Task EditData_CaseFive()
    {
        // Arrange
        Timestamp dateToSet = _staffClientMock.PersonStaffHistoryResponse
            .OrderByDescending(e => e.History.CreatedOn)
            .First()
            .History.CreatedOn
            .ToDateTime()
            .AddDays(-5)
            .ToTimestamp();
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _positionClientMock.PositionResponse.Id,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = dateToSet
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        // Act
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.UpdateEmployment(request, context));

        // Assert
        exception.Status.StatusCode.Should().Be(StatusCode.InvalidArgument);
        exception.Status.Detail.Should().Be($"Start work date should be greater than {dateToSet.ToDateTime().Date}");

        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetByPerson(_staffClientMock.PersonStaffFoundRequest);

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }

    [Fact(DisplayName = "UpdateEmployment should call set date")]
    public async Task EditData_CaseSix()
    {
        // Arrange
        Timestamp dateToSet = _staffClientMock.StaffSecond.History.CreatedOn
            .ToDateTime()
            .AddMonths(1)
            .ToTimestamp();
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _staffClientMock.StaffSecond.Position,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = dateToSet,
            Employment = _staffClientMock.StaffSecond.History.Employment
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewDate expectedSetDateRequest = new()
        {
            StaffId = request.EmploymentId,
            Date = dateToSet
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetDateAsync(expectedSetDateRequest);
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }

    [Fact(DisplayName = "UpdateEmployment should call set position")]
    public async Task EditData_CaseSeven()
    {
        // Arrange
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _positionClientMock.PositionResponse.Id,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = _staffClientMock.StaffSecond.History.CreatedOn,
            Employment = _staffClientMock.StaffSecond.History.Employment
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewPosition expectedPositionRequest = new()
        {
            StaffId = request.EmploymentId,
            Position = request.PositionId
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetPositionAsync(expectedPositionRequest);
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }
    
    [Fact(DisplayName = "UpdateEmployment should call set employment")]
    public async Task EditData_CaseEight()
    {
        // Arrange
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _staffClientMock.StaffSecond.Position,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = _staffClientMock.StaffSecond.History.CreatedOn,
            Employment = 80
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewEmployment expectedEmploymentRequest = new()
        {
            StaffId = request.EmploymentId,
            Employment = request.Employment
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetEmploymentAsync(expectedEmploymentRequest);
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }
    
    [Fact(DisplayName = "UpdateEmployment should call set manager")]
    public async Task EditData_CaseNine()
    {
        // Arrange
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _staffClientMock.StaffSecond.Position,
            ManagerId = _personClientMock.MentorFoundRequest.Value,
            MentorId = _personClientMock.MentorFoundRequest.Value,
            StartWork = _staffClientMock.StaffSecond.History.CreatedOn,
            Employment = _staffClientMock.StaffSecond.History.Employment
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewManager expectedManagerRequest = new()
        {
            StaffId = request.EmploymentId,
            Manager = _personClientMock.MentorFoundRequest.Value,
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetManagerAsync(expectedManagerRequest);
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetMentorAsync(Arg.Any<NewMentor>());
    }
    
    [Fact(DisplayName = "UpdateEmployment should call set mentor")]
    public async Task EditData_CaseTen()
    {
        // Arrange
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _staffClientMock.StaffSecond.Position,
            ManagerId = _personClientMock.ManagerFoundRequest.Value,
            MentorId = _personClientMock.ManagerFoundRequest.Value,
            StartWork = _staffClientMock.StaffSecond.History.CreatedOn,
            Employment = _staffClientMock.StaffSecond.History.Employment
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewMentor expectedMentorRequest = new()
        {
            StaffId = request.EmploymentId,
            Mentor = _personClientMock.ManagerFoundRequest.Value,
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.None()).SetDateAsync(Arg.Any<NewDate>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetPositionAsync(Arg.Any<NewPosition>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetEmploymentAsync(Arg.Any<NewEmployment>());
        await _staffClientMock.StaffClient.Received(Quantity.None()).SetManagerAsync(Arg.Any<NewManager>());
        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetMentorAsync(expectedMentorRequest);
    }
    
    [Fact(DisplayName = "UpdateEmployment should update all fields")]
    public async Task EditData_CaseEleven()
    {
        // Arrange
        Timestamp dateToSet = _staffClientMock.StaffSecond.History.CreatedOn
            .ToDateTime()
            .AddMonths(1)
            .ToTimestamp();
        EmploymentHistoryData request = new()
        {
            EmploymentId = _staffClientMock.StaffFoundRequest.Value,
            PositionId = _positionClientMock.PositionTwo.Id,
            ManagerId = _personClientMock.MentorFoundRequest.Value,
            MentorId = _personClientMock.ManagerFoundRequest.Value,
            StartWork = dateToSet,
            Employment = 80
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.UpdateEmployment));
        NewMentor expectedMentorRequest = new()
        {
            StaffId = request.EmploymentId,
            Mentor = _personClientMock.ManagerFoundRequest.Value,
        };
        NewManager expectedManagerRequest = new()
        {
            StaffId = request.EmploymentId,
            Manager = _personClientMock.ManagerFoundRequest.Value,
        };
        NewEmployment expectedEmploymentRequest = new()
        {
            StaffId = request.EmploymentId,
            Employment = request.Employment
        };
        NewDate expectedDateRequest = new()
        {
            StaffId = request.EmploymentId,
            Date = dateToSet
        };
        NewPosition expectedPositionRequest = new()
        {
            StaffId = request.EmploymentId,
            Position = request.PositionId
        };

        // Act
        await _service.UpdateEmployment(request, context);

        // Assert
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetById(_staffClientMock.StaffNotFoundRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());

        _personClientMock.PersonClient.Received(Quantity.Exactly(1)).GetAll(Arg.Any<Empty>());

        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetDateAsync(expectedDateRequest);
        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetPositionAsync(expectedPositionRequest);
        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetEmploymentAsync(expectedEmploymentRequest);
        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetManagerAsync(expectedManagerRequest);
        await _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).SetMentorAsync(expectedMentorRequest);
    }
}