using EMS.Gateway.API.DAL;
using EMS.Gateway.API.Models;
using EMS.Gateway.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Gateway.API.Repositories
{
	public class PositionsRepository: BaseRepository, IPositionsRepository
	{
		public PositionsRepository(IApplicationDbContext context): base(context) { }

		public async Task<int> AddAsync(Position position)
		{
            if (position is null)
            {
                throw new ArgumentNullException("Position cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(position.Name))
            {
                throw new ArgumentNullException("Position name cannot be empty");
            }
            if (position.TeamId == 0 || _context.Teams.FirstOrDefault(t => t.Id == position.TeamId) is null ||
                _context.Teams.First(t => t.Id == position.TeamId) != position.Team)
            {
                throw new DbUpdateException("Cannot add position with non exists team");
            }
            _context.Positions.Add(position);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateAsync(Position position)
		{
            if (position is null)
            {
                throw new ArgumentNullException("Position cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(position.Name))
            {
                throw new ArgumentNullException("Position name cannot be empty");
            }
            if(position.TeamId == 0 || _context.Teams.FirstOrDefault(t=> t.Id == position.TeamId) is null ||
                _context.Teams.First(t => t.Id == position.TeamId) != position.Team)
            {
                throw new DbUpdateException("Cannot add position with non exists team");
            }
            _context.Positions.Update(position);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteAsync(Position position)
		{
            _context.Positions.Remove(position);
			return await _context.SaveChangesAsync();
		}

		public async Task<Position> GetAsync(long id)
		{
			return await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
		}

		public IQueryable<Position> GetAll()
		{
			return _context.Positions.AsQueryable();
		}
	}
}
