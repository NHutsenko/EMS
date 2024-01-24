namespace EMS.Staff.Application.Interfaces;

public interface IStaffRepository
{
    Task<EMS.Staff.Domain.Staff> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<EMS.Staff.Domain.Staff>> GetByPersonAsync(int personId, CancellationToken cancellationToken);
    Task<IEnumerable<EMS.Staff.Domain.Staff>> GetByManagerAsync(int managerId, CancellationToken cancellationToken);
    Task<IEnumerable<EMS.Staff.Domain.Staff>> GetAllAsync(CancellationToken cancellationToken);
    Task<int> CreateAsync(int position, int manager, int person, int employment, DateTime createdOn, int? mentor, CancellationToken cancellationToken);
    Task SetManagerAsync(int staffId, int manager, CancellationToken cancellationToken);
    Task SetPositionAsync(int staffId, int position, CancellationToken cancellationToken);
    Task SetDateAsync(int staffId, DateTime createdOn, CancellationToken cancellationToken);
    Task SetEmploymentAsync(int staffId, int employment, CancellationToken cancellationToken);
    Task SetMentorAsync(int staffId, int? mentor, CancellationToken cancellationToken);
    Task ThrowExceptionIfDateIsWrongAsync(int person, DateTime date, CancellationToken cancellationToken);
}