using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Gateway.API.Models;

namespace EMS.Gateway.API.DAL.Repositories.Interfaces
{
    public interface IStaffRepository
    {
        Task<int> AddAsync(Staff staff);
        Task<int> UpdateAsync(Staff staff);
        Task<int> DeleteAsync(Staff staff);
        public IQueryable<Staff> GetByPerson(long personId);
        public IQueryable<Staff> GetByManager(long managerId);
        public IQueryable<Staff> GetAll();
    }
}
