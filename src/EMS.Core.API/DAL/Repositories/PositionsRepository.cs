using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Core.API.DAL.Repositories
{
    public class PositionsRepository : BaseRepository, IPositionsRepository
    {
        public PositionsRepository(IApplicationDbContext context, IDateTimeUtil dateTimeUtil) : base(context, dateTimeUtil) { }

        public virtual async Task<int> AddAsync(Position position)
        {
            if (position is null)
            {
                throw new NullReferenceException("Position cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(position.Name))
            {
                throw new ArgumentException("Position name cannot be empty");
            }
            if(_context.Positions.Any(e => e.Name == position.Name))
            {
                throw new ArgumentException("Position with the same name already exists");
            }
            if(position.HourRate <= 0)
            {
                throw new ArgumentException("Hour rate cannot be 0 or less");
            }
            if (position.TeamId == 0 || !_context.Teams.Any(t => t.Id == position.TeamId))
            {
                throw new ArgumentException("Cannot add position with non specified team");
            }
            position.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.Positions.Add(position);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(Position position)
        {
            if (position is null)
            {
                throw new NullReferenceException("Position cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(position.Name))
            {
                throw new ArgumentException("Position name cannot be empty");
            }
            if (_context.Positions.Any(e => e.Name == position.Name))
            {
                throw new ArgumentException("Position with the same name already exists");
            }
            if (position.HourRate <= 0)
            {
                throw new ArgumentException("Hour rate cannot be 0 or less");
            }
            if (position.TeamId == 0 || !_context.Teams.Any(t => t.Id == position.TeamId))
            {
                throw new ArgumentException("Cannot update position with non exists team");
            }
            _context.Positions.Update(position);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(Position position)
        {
            if(position is null)
            {
                throw new NullReferenceException("Position cannot be empty");
            }
            if (_context.Teams.Any(e => e.Id == position.TeamId))
            {
                throw new InvalidOperationException("Cannot delete position related to team");
            }
            if (_context.Staff.Any(e => e.PositionId == position.Id))
            {
                throw new InvalidOperationException("Cannot delete position related to staff");
            }
            _context.Positions.Remove(position);
            return await _context.SaveChangesAsync();
        }

        public virtual Position Get(long id)
        {
            return _context.Positions.FirstOrDefault(p => p.Id == id);
        }

        public virtual IQueryable<Position> GetAll()
        {
            return _context.Positions.Select(e => e);
        }
    }
}
