using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories
{
    public class OtherPaymentsRepository: BaseRepository, IOtherPaymentsRepository
    {
        public  OtherPaymentsRepository(IApplicationDbContext applicationDbContext) : base(applicationDbContext, null) { }

        public async Task<int> AddAsync(OtherPayment otherPayment)
        {
            _context.OtherPayments.Add(otherPayment);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(OtherPayment otherPayment)
        {
            _context.OtherPayments.Update(otherPayment);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(OtherPayment otherPayment)
        {
            _context.OtherPayments.Remove(otherPayment);
            return await _context.SaveChangesAsync();
        }

        public IQueryable<OtherPayment> GetByPersonId(long personId)
        {
            return _context.OtherPayments.Where(e => e.PersonId == personId);
        }

        public IQueryable<OtherPayment> GetByPersonIdAndDateRange(long personId, DateTime startRange, DateTime endRange)
        {
            return _context.OtherPayments.Where(e => e.PersonId == personId && e.CreatedOn >= startRange && e.CreatedOn <= endRange);
        }
    }
}
