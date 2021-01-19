using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Core.API.DAL.Repositories
{
    public class TeamsRepository: BaseRepository, ITeamsRepository
	{
		public TeamsRepository(IApplicationDbContext context, IDateTimeUtil dateTimeUtil): base(context, dateTimeUtil) { }

		public async Task<int> AddAsync(Team team)
		{
            if(team is null)
            {
                throw new NullReferenceException("Team cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(team.Name))
            {
                throw new ArgumentNullException(nameof(team.Name), "Team Name cannot be empty");
            }
            _context.Teams.Add(team);
			return await _context.SaveChangesAsync();
		}


		public async Task<int> UpdateAsync(Team team)
		{
            if (team is null)
            {
                throw new NullReferenceException("Team cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(team.Name))
            {
                throw new ArgumentNullException(nameof(team.Name), "Team Name cannot be empty");
            }
            _context.Teams.Update(team);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteAsync(Team team)
		{
            if (team is null)
            {
                throw new NullReferenceException("Team cannot be empty");
            }
            if (!(_context.Positions.FirstOrDefault(p => p.TeamId == team.Id) is null) )
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
    }
}
