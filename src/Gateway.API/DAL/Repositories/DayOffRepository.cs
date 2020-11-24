using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Gateway.API.DAL.Repositories.Interfaces;
using EMS.Gateway.API.Models;

namespace EMS.Gateway.API.DAL.Repositories
{
    public class DayOffRepository : BaseRepository, IDayOffRepository
    {
        public DayOffRepository(IApplicationDbContext context) : base(context) { }

        public async Task<int> AddAsync(DayOff dayOff)
        {
            if (_context.DaysOff.Any(e => e.CreatedOn.Date == dayOff.CreatedOn.Date && e.StaffId == dayOff.StaffId))
            {
                return 0;
            }
            _context.DaysOff.Add(dayOff);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(DayOff dayOff)
        {
            _context.DaysOff.Update(dayOff);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(DayOff dayOff)
        {
            if (_context.DaysOff.Any(e => e.Id == dayOff.Id))
            {
                throw new Exception("An error occured while deleteing day off record with existing staff");
            }
            _context.DaysOff.Remove(dayOff);
            return await _context.SaveChangesAsync();
        }

        public IQueryable<DayOff> GetAll()
        {
            return _context.DaysOff.AsQueryable();
        }

        public IQueryable<DayOff> GetByStaffId(long staffId)
        {
            if(staffId == 0)
            {
                throw new ArgumentException("Cannot get records for staff Id equals to 0");
            }
            return _context.DaysOff.Where(e => e.StaffId == staffId);
        }
    }
}
