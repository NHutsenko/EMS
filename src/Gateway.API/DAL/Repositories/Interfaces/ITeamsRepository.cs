using EMS.Gateway.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Gateway.API.Repositories.Interfaces
{
	public interface ITeamsRepository
	{
		Task<int> AddAsync(Team team);
		Task<int> UpdateAsync(Team team);
		Task<int> DeleteAsync(Team team);
		Task<Team> GetAsync(long teamId);
		IQueryable<Team> GetAll();
	}
}
