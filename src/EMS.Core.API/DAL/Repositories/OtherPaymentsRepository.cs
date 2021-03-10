using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.DAL.Repositories
{
    public class OtherPaymentsRepository: BaseRepository, IOtherPaymentsRepository
    {
        public  OtherPaymentsRepository(IApplicationDbContext applicationDbContext, IDateTimeUtil dateTimeUtil) : base(applicationDbContext, dateTimeUtil) { }

        public virtual async Task<int> AddAsync(OtherPayment otherPayment)
        {
            ValidateData(otherPayment);
            _context.OtherPayments.Add(otherPayment);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(OtherPayment otherPayment)
        {
            ValidateData(otherPayment);
            _context.OtherPayments.Update(otherPayment);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(OtherPayment otherPayment)
        {
            if(otherPayment is null)
            {
                throw new NullReferenceException("Other payment cannot be empty");
            }
            DateTime currentDate = _dateTimeUtil.GetCurrentDateTime();
            DateTime currentMonth = new DateTime(currentDate.Year, currentDate.Month + 1, 1);
            if(otherPayment.CreatedOn < currentMonth)
            {
                throw new InvalidOperationException("Cannot delete history record");
            }
            _context.OtherPayments.Remove(otherPayment);
            return await _context.SaveChangesAsync();
        }

        public virtual IQueryable<OtherPayment> GetByPersonId(long personId)
        {
            return _context.OtherPayments.Where(e => e.PersonId == personId);
        }

        public virtual IQueryable<OtherPayment> GetByPersonIdAndDateRange(long personId, DateTime startRange, DateTime endRange)
        {
            if(startRange > endRange)
            {
                throw new ArgumentException("Wrong range date. Start date cannot be greater than end date");
            }
            return _context.OtherPayments.Where(e => e.PersonId == personId && e.CreatedOn >= startRange && e.CreatedOn <= endRange);
        }

        private static void ValidateData(OtherPayment otherPayment)
        {
            if (otherPayment is null)
            {
                throw new NullReferenceException("Other payment data cannot be empty");
            }
            if (otherPayment.Value == 0)
            {
                throw new ArgumentException("Summ of payment cannot be equal to zero");
            }
            if (string.IsNullOrWhiteSpace(otherPayment.Comment))
            {
                throw new ArgumentException("Comment cannot be empty");
            }
            if (otherPayment.CreatedOn == DateTime.MinValue)
            {
                throw new ArgumentException("Date of payment must be specified");
            }
            if(otherPayment.PersonId == 0)
            {
                throw new ArgumentException("Person identifier cannot be empty");
            }
        }
    }
}
