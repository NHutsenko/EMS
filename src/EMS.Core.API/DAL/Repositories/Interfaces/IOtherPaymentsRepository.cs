using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories.Interfaces
{
    public interface IOtherPaymentsRepository
    {
        Task<int> AddAsync(OtherPayment otherPayment);
        Task<int> UpdateAsync(OtherPayment otherPayment);
        Task<int> DeleteAsync(OtherPayment otherPayment);
        IQueryable<OtherPayment> GetByPersonId(long personId);
        IQueryable<OtherPayment> GetByPersonIdAndDateRange(long personId, DateTime startRange, DateTime endRange);
    }
}
