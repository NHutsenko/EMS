using EMS.Gateway.API.DAL;
using EMS.Gateway.API.Models;
using EMS.Gateway.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Gateway.API.Repositories
{
	public class TeamsRepository: BaseRepository, ITeamsRepository
	{
		public TeamsRepository(ApplicationDbContext context): base(context) { }

		public async Task<int> AddAsync(Team team)
		{
			_context.Entry(team).State = EntityState.Added;
			return await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateAsync(Team team)
		{
			_context.Entry(team).State = EntityState.Modified;
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteAsync(Team team)
		{
			_context.Entry(team).State = EntityState.Deleted;
			return await _context.SaveChangesAsync();
		}

		public async Task<Team> GetAsync(long teamId)
		{
			return await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
		}

		public IQueryable<Team> GetAll()
		{
			return  _context.Teams.AsQueryable();
		}
	}
}
