using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Gateway.API.Models;

namespace EMS.Gateway.API.DAL.Repositories.Interfaces
{
    public interface IDayOffRepository
    {
        Task<int> AddAsync(DayOff dayOff);
        Task<int> UpdateAsync(DayOff dayOff);
        IQueryable<DayOff> GetAll();
        IQueryable<DayOff> GetByStaffId(long staffId);
        IQueryable<DayOff> GetByDateRange(DateTime start, DateTime end);
        IQueryable<DayOff> GetByDateRangeAndStaffId(DateTime start, DateTime end, long staffId);
    }
}
