using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Gateway.API.Models;
using EMS.Gateway.API.Repositories;

namespace EMS.Gateway.API.DAL.Repositories.Interfaces
{
    public interface IDayOffRepository
    {
        Task<int> AddAsync(DayOff dayOff);
        Task<int> UpdateAsync(DayOff dayOff);
        Task<int> DeleteAsync(DayOff dayOff);
        IQueryable<DayOff> GetAll();
        IQueryable<DayOff> GetByStaffId(long staffId);
    }
}
