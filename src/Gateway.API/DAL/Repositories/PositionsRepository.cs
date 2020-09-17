using EMS.Gateway.API.DAL;
using EMS.Gateway.API.Models;
using EMS.Gateway.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Gateway.API.Repositories
{
	public class PositionsRepository: BaseRepository, IPositionsRepository
	{
		public PositionsRepository(ApplicationDbContext context): base(context) { }

		public async Task<int> AddAsync(Position position)
		{
			_context.Entry(position).State = EntityState.Added;
			return await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateAsync(Position position)
		{
			_context.Entry(position).State = EntityState.Modified;
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteAsync(Position position)
		{
			_context.Entry(position).State = EntityState.Deleted;
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
