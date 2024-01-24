using EMS.Staff.Application.Interfaces;
using NSubstitute;

namespace EMS.Staff.Application.Tests.Mocks;

internal sealed class StaffRepositoryMock
{
    public IStaffRepository StaffRepository { get; init; }
    public Domain.Staff ActualStaff { get; init; }

    public StaffRepositoryMock()
    {
        StaffRepository = Substitute.For<IStaffRepository>();

        ActualStaff = new Domain.Staff
        {
            Id = 1,
            ManagerId = 1,
            PositionId = 1,
            History = new Domain.History
            {
                Id = 1,
                StaffId = 1,
                CreatedOn = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Employment = 1,
                PersonId = 1,
            }
        };

        StaffRepository.CreateAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        StaffRepository.SetDateAsync(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        StaffRepository.SetEmploymentAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
           .Returns(Task.CompletedTask);
        StaffRepository.SetManagerAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
           .Returns(Task.CompletedTask);
        StaffRepository.SetMentorAsync(Arg.Any<int>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
           .Returns(Task.CompletedTask);
        StaffRepository.SetPositionAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
           .Returns(Task.CompletedTask);

        StaffRepository.GetByIdAsync(ActualStaff.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(ActualStaff));
    }
}