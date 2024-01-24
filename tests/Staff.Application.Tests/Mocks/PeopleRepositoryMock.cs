using EMS.Staff.Application.Interfaces;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Staff.Application.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal sealed class PeopleRepositoryMock
{
    public IPeopleRepository PeopleRepository { get; init; }
    public static int PersonId => 1;
    public static int ManagerId => 2;
    public static int MentorId = 3;

    public PeopleRepositoryMock()
    {
        PeopleRepository = Substitute.For<IPeopleRepository>();

        PeopleRepository.CheckPeopleAsync(Arg.Any<int?>(), Arg.Any<int>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
    }
}