using EMS.Core.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Core.API.DAL.Repositories.Interfaces
{
    public interface IPositionsRepository
	{
		Task<int> AddAsync(Position position);
		Task<int> UpdateAsync(Position position);
		Task<int> DeleteAsync(Position position);
		Position Get(long positionId);
		IQueryable<Position> GetAll();
	}
}
