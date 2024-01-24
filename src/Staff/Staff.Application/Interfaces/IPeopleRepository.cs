namespace EMS.Staff.Application.Interfaces;

public interface IPeopleRepository
{
    Task CheckPeopleAsync(int? personId, int managerId, int? mentorId, CancellationToken cancellationToken);
}