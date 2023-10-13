using EMS.EmploymentHistory.Tests.Mocks;
using EMS.Protos;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ReceivedExtensions;
using EmploymentHistoryService = EMS.EmploymentHistory.Services.EmploymentHistoryService;

namespace EMS.EmploymentHistory.Tests;

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
        staffClientCalls.ToArray()[0].GetMethodInfo().Name.Should().Be(nameof(_staffClientMock.StaffClient.CreateAsync));
        staffClientCalls.ToArray()[0].GetArguments()[0].Should().Be(newStaffRequest);
        staffClientCalls.ToArray()[1].GetMethodInfo().Name.Should().Be(nameof(_staffClientMock.StaffClient.CreateHistoryAsync));
        staffClientCalls.ToArray()[1].GetArguments()[0].Should().Be(newHistoryRequest);
    }
    
    [Fact(DisplayName = "CreateEmployment should throw exception that person not found")]
    public async Task CreateData_TestCaseTwo()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        NewEmploymentHistory request = new()
        {
            PersonId = _personClientMock.PersonNotFoundRequest.Value,
            StartWork = new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
            PositionId = _positionClientMock.PositionResponse.Id,
            Employment = 100,
            ManagerId = _personClientMock.ManagerResponse.Id
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.CreateEmployment));
        Int32Value personRequest = new()
        {
            Value = request.PersonId
        };

        // Act
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.CreateEmployment(request, context));
        
        // Assert
        exception.Status.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Status.Detail.Should().Be($"Person with id {personRequest.Value} not found");
        
        var personClientCalls = _personClientMock.PersonClient.ReceivedCalls();
        personClientCalls.ToArray()[0].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[0].GetArguments()[0].Should().Be(personRequest);

        _positionClientMock.PositionClient.Received(Quantity.None()).GetAll(new Empty());
        
        _staffClientMock.StaffClient.Received(Quantity.None()).GetByPerson(Arg.Any<Int32Value>());
        _staffClientMock.StaffClient.Received(Quantity.None()).GetByPerson(Arg.Any<Int32Value>());
        _staffClientMock.StaffClient.ReceivedCalls().Count().Should().Be(0);
    }
    
    [Fact(DisplayName = "CreateEmployment should throw exception that manager not found")]
    public async Task CreateData_TestCaseThree()
    {
        // Arrange
        _service = new EmploymentHistoryService(_personClientMock.PersonClient, _positionClientMock.PositionClient, _staffClientMock.StaffClient);
        NewEmploymentHistory request = new()
        {
            PersonId = _personClientMock.PersonFoundRequest.Value,
            StartWork = new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
            PositionId = _positionClientMock.PositionResponse.Id,
            Employment = 100,
            ManagerId = _personClientMock.PersonNotFoundRequest.Value
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
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.CreateEmployment(request, context));
        
        
        // Assert
        exception.Should().NotBe(null);
        exception.Status.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Status.Detail.Should().Be($"Person with id {managerRequest.Value} not found");

        var personClientCalls = _personClientMock.PersonClient.ReceivedCalls();
        personClientCalls.ToArray()[0].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[0].GetArguments()[0].Should().Be(personRequest);
        personClientCalls.ToArray()[1].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[1].GetArguments()[0].Should().Be(managerRequest);

        _positionClientMock.PositionClient.Received(Quantity.None()).GetAll(new Empty());
        
        _staffClientMock.StaffClient.Received(Quantity.None()).GetByPerson(Arg.Any<Int32Value>());
        _staffClientMock.StaffClient.ReceivedCalls().Count().Should().Be(0);
    }
    
    [Fact(DisplayName = "CreateEmployment should throw exception that position not found")]
    public async Task CreateData_TestCaseFour()
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
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.CreateEmployment(request, context));
        
        // Assert
        exception.Should().NotBe(null);
        exception.Status.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Status.Detail.Should().Be($"Position with id {request.PositionId} does not exists");
        
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
    public async Task CreateData_TestCaseFive()
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
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.CreateEmployment(request, context));
        
        // Assert
        exception.Should().NotBe(null);
        exception.Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
        exception.Status.Detail.Should().Be($"Employment period for date {wrongDate.ToDateTime().Date} already exists");
        
        var personClientCalls = _personClientMock.PersonClient.ReceivedCalls();
        personClientCalls.ToArray()[0].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[0].GetArguments()[0].Should().Be(personRequest);
        personClientCalls.ToArray()[1].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[1].GetArguments()[0].Should().Be(managerRequest);

        _positionClientMock.PositionClient.Received(Quantity.Exactly(1)).GetAll(new Empty());
        
        _staffClientMock.StaffClient.Received(Quantity.Exactly(1)).GetByPerson(_staffClientMock.PersonStaffFoundRequest);
        _staffClientMock.StaffClient.ReceivedCalls().Count().Should().Be(0);
    }
    
     [Fact(DisplayName = "CreateEmployment should throw exception that mentor not found")]
    public async Task CreateData_TestCaseSix()
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
            ManagerId = _personClientMock.ManagerResponse.Id,
            MentorId = _personClientMock.PersonNotFoundRequest.Value
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
        Int32Value mentorRequest = new()
        {
            Value = request.MentorId.Value
        };

        // Act
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() => _service.CreateEmployment(request, context));
        
        // Assert
        exception.Should().NotBe(null);
        exception.Status.StatusCode.Should().Be(StatusCode.NotFound);
        exception.Status.Detail.Should().Be($"Person with id {mentorRequest.Value} not found");
        
        var personClientCalls = _personClientMock.PersonClient.ReceivedCalls();
        personClientCalls.ToArray()[0].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[0].GetArguments()[0].Should().Be(personRequest);
        personClientCalls.ToArray()[1].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[1].GetArguments()[0].Should().Be(managerRequest);
        personClientCalls.ToArray()[2].GetMethodInfo().Name.Should().Be(nameof(_personClientMock.PersonClient.GetAsync));
        personClientCalls.ToArray()[2].GetArguments()[0].Should().Be(mentorRequest);

        _positionClientMock.PositionClient.Received(Quantity.None()).GetAll(new Empty());
        
        _staffClientMock.StaffClient.Received(Quantity.None()).GetByPerson(_staffClientMock.PersonStaffFoundRequest);
        _staffClientMock.StaffClient.ReceivedCalls().Count().Should().Be(0);
    }
}