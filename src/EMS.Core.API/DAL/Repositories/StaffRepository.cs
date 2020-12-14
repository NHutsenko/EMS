using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.DAL.Repositories
{
    public class StaffRepository: BaseRepository, IStaffRepository
    {
        public StaffRepository(IApplicationDbContext context) : base(context) { }

        public async Task<int> AddAsync(Staff staff)
        {
            if(staff is null)
            {
                throw new ArgumentNullException("Staff entity cannot be null");
            }

            if(staff.ManagerId == 0)
            {
                throw new ArgumentException("ManagerId in staff entity cannot be 0");
            }

            if(staff.PositionId == 0)
            {
                throw new ArgumentException("PositionId in staff entity cannot be 0");
            }

            if(_context.Staff.Any(e => e.CreatedOn >= staff.CreatedOn && e.PersonId == staff.PersonId))
            {
                throw new ArgumentException("Cannot add new staff record into existing work period");
            }

            _context.Staff.Add(staff);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Staff staff)
        {
            if (staff is null)
            {
                throw new ArgumentNullException("Staff entity cannot be null");
            }

            if (staff.ManagerId == 0)
            {
                throw new ArgumentException("ManagerId in staff entity cannot be 0");
            }

            if (staff.PositionId == 0)
            {
                throw new ArgumentException("PositionId in staff entity cannot be 0");
            }

            bool hasUpdateDateError = _context.Staff.Where(e => e.Id < staff.Id && e.PersonId == staff.PersonId)
                .OrderBy(e => e.CreatedOn)
                .Any(e => e.CreatedOn >= staff.CreatedOn);
            if (hasUpdateDateError)
            {
                throw new ArgumentException("Cannot update existing staff record into existing work period");
            }
            _context.Staff.Update(staff);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Staff staff)
        {
            if(staff.CreatedOn <= DateTime.Now.Date)
            {
                throw new DbUpdateException("Cannot delete history record");
            }
            _context.Staff.Remove(staff);
            return await _context.SaveChangesAsync();
        }

        public IQueryable<Staff> GetAll()
        {
            return _context.Staff.Select(e => e);
        }

        public IQueryable<Staff> GetByPersonId(long personId)
        {
            return _context.Staff.Where(s => s.PersonId == personId);
        }

        public IQueryable<Staff> GetByManagerId(long managerId)
        {
            return _context.Staff.Where(s => s.ManagerId == managerId);
        }
    }
}
