using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories.Interfaces
{
    public interface IHolidaysRepository
    {
        Task<int> AddAsync(Holiday holiday);
        Task<int> UpdateAsync(Holiday holiday);
        Task<int> DeleteAsync(Holiday holiday);
        IQueryable<Holiday> GetAll();
        IQueryable<Holiday> GetByDateRange(DateTime startRange, DateTime endRange);
    }
}
