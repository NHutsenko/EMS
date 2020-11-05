using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Gateway.API.DAL.Repositories.Interfaces;
using EMS.Gateway.API.Models;
using EMS.Gateway.API.Repositories;

namespace EMS.Gateway.API.DAL.Repositories
{
    public class StaffRepository: BaseRepository, IStaffRepository
    {
        public StaffRepository(IApplicationDbContext context) : base(context) { }

        public async Task<int> AddAsync(Staff staff)
        {
            _context.Staff.Add(staff);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Staff staff)
        {
            _context.Staff.Update(staff);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Staff staff)
        {
            _context.Staff.Remove(staff);
            return await _context.SaveChangesAsync();
        }

        public IQueryable<Staff> GetAll()
        {
            return _context.Staff.AsQueryable();
        }

        public IQueryable<Staff> GetByPerson(long personId)
        {
            return _context.Staff.Where(s => s.PersonId == personId);
        }

        public IQueryable<Staff> GetByManager(long managerId)
        {
            return _context.Staff.Where(s => s.ManagerId == managerId);
        }
    }
}
