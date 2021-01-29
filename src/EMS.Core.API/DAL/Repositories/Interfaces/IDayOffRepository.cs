using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories.Interfaces
{
    public interface IDayOffRepository
    {
        Task<int> AddAsync(DayOff dayOff);
        Task<int> UpdateAsync(DayOff dayOff);
        Task<int> DeleteAsync(DayOff dayOff);
        IQueryable<DayOff> GetByPersonId(long personId);
        IQueryable<DayOff> GetByDateRange(DateTime start, DateTime end);
        IQueryable<DayOff> GetByDateRangeAndPersonId(DateTime start, DateTime end, long personId);
    }
}
