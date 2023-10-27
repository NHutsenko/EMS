using EMS.Structure.Models;

namespace EMS.Structure.Interfaces;

public interface IPositionRepository
{
    Task<IEnumerable<Position>> GetAllAsync(CancellationToken cancellationToken);
    Task<int> CreateAsync(string name, IDictionary<int, decimal> grades, CancellationToken cancellationToken);
    Task UpdateSalaryAsync(int positionId, KeyValuePair<int, decimal> gradeData, CancellationToken cancellationToken);
}