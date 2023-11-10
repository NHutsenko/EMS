using EMS.Structure.Models;

namespace EMS.Structure.Application.Interfaces;

public interface ITeamRepository
{
    Task AddMemberAsync(int teamId, int memberId, int employment, DateTime startWork, CancellationToken cancellationToken);
    Task<int> CreateAsync(string name, CancellationToken cancellationToken);
    Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken);
    Task SetEndWorkAsync(int teamId, int memberId, DateTime date, CancellationToken cancellationToken);
    Task UpdateEmploymentAsync(int teamId, int memberId, int employment, CancellationToken cancellationToken);
    Task UpdateNameAsync(int id, string name, CancellationToken cancellationToken);
}