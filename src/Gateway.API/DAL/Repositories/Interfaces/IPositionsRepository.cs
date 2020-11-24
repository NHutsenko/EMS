using EMS.Gateway.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Gateway.API.Repositories.Interfaces
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
