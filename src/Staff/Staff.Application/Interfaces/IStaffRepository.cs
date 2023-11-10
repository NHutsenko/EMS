namespace EMS.Staff.Application.Interfaces;

public interface IStaffRepository
{
    Task<EMS.Staff.Domain.Staff> GetByHistoryIdAsync(int historyId, CancellationToken cancellationToken);
    Task<IEnumerable<EMS.Staff.Domain.Staff>> GetByPersonAsync(int personId, CancellationToken cancellationToken);
    Task<IEnumerable<EMS.Staff.Domain.Staff>> GetByManagerAsync(int managerId, CancellationToken cancellationToken);
    Task<IEnumerable<EMS.Staff.Domain.Staff>> GetAllAsync(CancellationToken cancellationToken);
    Task<int> CreateAsync(int position, int manager, CancellationToken cancellationToken);
    Task CreateHistoryAsync(int staffId, int person, int? mentor, int employment, DateTime createdOn, CancellationToken cancellationToken);
    Task SetManagerAsync(int staffId, int manager, CancellationToken cancellationToken);
    Task SetPositionAsync(int staffId, int position, CancellationToken cancellationToken);
    Task SetDateAsync(int staffId, DateTime createdOn, CancellationToken cancellationToken);
    Task SetEmploymentAsync(int staffId, int employment, CancellationToken cancellationToken);
    Task SetMentorAsync(int staffId, int? mentor, CancellationToken cancellationToken);
}