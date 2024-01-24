using EMS.Protos;
using EMS.Staff.Application.Tests.Mocks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using StaffService = EMS.Staff.Application.Services.StaffService;

namespace EMS.Staff.Application.Tests;

[ExcludeFromCodeCoverage]
public partial class StaffServiceTests
{
    private StaffService _service;
    private readonly PositionRepositoryMock _positionRepositoryMock = new();
    private readonly PeopleRepositoryMock _peopleRepositoryMock = new();
    private readonly StaffRepositoryMock _staffRepositoryMock = new();

    [Fact(DisplayName = "CreateEmployment should create employment record")]
    public async Task CreateData_TestCaseOne()
    {
        // Arrange
        _service = new StaffService(_staffRepositoryMock.StaffRepository, _peopleRepositoryMock.PeopleRepository, _positionRepositoryMock.PositionRepository);
        NewStaff request = new()
        {
            Person = PeopleRepositoryMock.PersonId,
            StartWork = new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
            Position = PositionRepositoryMock.ExistingPosition,
            Employment = 100,
            Manager = PeopleRepositoryMock.ManagerId
        };
        ServerCallContext context = GrpcCoreMock.GetCallContext(nameof(_service.Create));

        // Act
        Int32Value response = await _service.Create(request, context);

        // Assert
        await _peopleRepositoryMock.PeopleRepository.Received(1).CheckPeopleAsync(request.Person, request.Manager, null, Arg.Any<CancellationToken>());
        await _positionRepositoryMock.PositionRepository.Received(1).ThrowExceptionIfPositionNotFoundAsync(request.Position, Arg.Any<CancellationToken>());
        await _staffRepositoryMock.StaffRepository.Received(1).ThrowExceptionIfDateIsWrongAsync(request.Person, request.StartWork.ToDateTime(), Arg.Any<CancellationToken>());
        await _staffRepositoryMock.StaffRepository.Received(1).CreateAsync(request.Position, request.Manager, request.Person, request.Employment, request.StartWork.ToDateTime(), null, Arg.Any<CancellationToken>());
    }
}
