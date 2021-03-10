using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Core.API.DAL.Repositories
{
    public class TeamsRepository : BaseRepository, ITeamsRepository
    {
        public TeamsRepository(IApplicationDbContext context, IDateTimeUtil dateTimeUtil) : base(context, dateTimeUtil) { }

        public virtual async Task<int> AddAsync(Team team)
        {
            if (team is null)
            {
                throw new NullReferenceException("Team cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(team.Name))
            {
                throw new ArgumentException("Team Name cannot be empty");
            }
            if (_context.Teams.Any(e => e.Name == team.Name))
            {
                throw new ArgumentException("Team with the same name already exists");
            }
            team.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.Teams.Add(team);
            return await _context.SaveChangesAsync();
        }


        public virtual async Task<int> UpdateAsync(Team team)
        {
            if (team is null)
            {
                throw new NullReferenceException("Team cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(team.Name))
            {
                throw new ArgumentException("Team Name cannot be empty");
            }
            if (_context.Teams.Any(e => e.Name == team.Name))
            {
                throw new ArgumentException("Team with the same name already exists");
            }
            _context.Teams.Update(team);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(Team team)
        {
            if (team is null)
            {
                throw new NullReferenceException("Team cannot be empty");
            }
            if (_context.Positions.Any(p => p.TeamId == team.Id))
            {
                throw new InvalidOperationException("Team cannot be deleted because of this team has positions");
            }
            _context.Teams.Remove(team);
            return await _context.SaveChangesAsync();
        }

        public virtual Team Get(long teamId)
        {
            return _context.Teams.FirstOrDefault(t => t.Id == teamId);
        }

        public virtual IQueryable<Team> GetAll()
        {
            return _context.Teams.Select(e => e);
        }
    }
}
