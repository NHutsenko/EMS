using EMS.Core.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Core.API.Repositories.Interfaces
{
    public interface ITeamsRepository
	{
		Task<int> AddAsync(Team team);
		Task<int> UpdateAsync(Team team);
		Task<int> DeleteAsync(Team team);
		Team Get(long teamId);
		IQueryable<Team> GetAll();
        IQueryable<Position> GetPositionsByTeamId(long teamId);
	}
}
