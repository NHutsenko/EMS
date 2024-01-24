namespace EMS.Staff.Application.Interfaces;

public interface IPositionRepository
{
    Task ThrowExceptionIfPositionNotFoundAsync(int positionId, CancellationToken cancellationToken);
}