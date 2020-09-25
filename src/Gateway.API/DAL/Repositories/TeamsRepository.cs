using EMS.Gateway.API.DAL;
using EMS.Gateway.API.Models;
using EMS.Gateway.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Gateway.API.Repositories
{
	public class TeamsRepository: BaseRepository, ITeamsRepository
	{
		public TeamsRepository(IApplicationDbContext context): base(context) { }

		public async Task<int> AddAsync(Team team)
		{
            if (string.IsNullOrWhiteSpace(team.Name))
            {
                throw new ArgumentNullException(nameof(team.Name), "Team Name cannot be empty");
            }
            _context.Teams.Add(team);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateAsync(Team team)
		{
            if (string.IsNullOrWhiteSpace(team.Name))
            {
                throw new ArgumentNullException(nameof(team.Name), "Team Name cannot be empty");
            }
            _context.Teams.Update(team);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteAsync(Team team)
		{
            if(!(_context.Positions.FirstOrDefault(p => p.TeamId == team.Id) is null) )
            {
                throw new DbUpdateException("Team cannot be deleted because of this team has positions");
            }
            _context.Teams.Remove(team);
			return await  _context.SaveChangesAsync();
		}

		public Team Get(long teamId)
		{
			return _context.Teams.FirstOrDefault(t => t.Id == teamId);
		}

		public IQueryable<Team> GetAll()
		{
            return _context.Teams.Select(e => e);
		}

        public IQueryable<Position> GetPositionsByTeamId(long teamId)
        {
            return _context.Positions.Where(p => p.TeamId == teamId);
        }

    }
}
