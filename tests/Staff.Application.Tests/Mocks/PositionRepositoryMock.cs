using EMS.Staff.Application.Interfaces;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Staff.Application.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal sealed class PositionRepositoryMock
{
    public IPositionRepository PositionRepository { get; init; }
    public static int ExistingPosition = 1;
    public PositionRepositoryMock()
    {
        PositionRepository = Substitute.For<IPositionRepository>();

        PositionRepository.ThrowExceptionIfPositionNotFoundAsync(ExistingPosition, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
    }
}