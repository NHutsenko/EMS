using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.DAL.Repositories
{
    public class HolidaysRepository : BaseRepository, IHolidaysRepository
    {
        public HolidaysRepository(IApplicationDbContext applicationDbContext, IDateTimeUtil dateTimeUtil) : base(applicationDbContext, dateTimeUtil) { }

        public async Task<int> AddAsync(Holiday holiday)
        {
            ValidateData(holiday);
            holiday.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.Holidays.Add(holiday);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Holiday holiday)
        {
            ValidateData(holiday);
            _context.Holidays.Update(holiday);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Holiday holiday)
        {
            if(holiday is null)
            {
                throw new NullReferenceException("Holiday cannot be empty");
            }
            DateTime nextMonth = new DateTime(_dateTimeUtil.GetCurrentDateTime().Year, _dateTimeUtil.GetCurrentDateTime().Month + 1, 1);
            if(holiday.HolidayDate < nextMonth)
            {
                throw new InvalidOperationException("Cannot delete history record");
            }
            _context.Holidays.Remove(holiday);
            return await _context.SaveChangesAsync();
        }

        public IQueryable<Holiday> GetAll()
        {
            return _context.Holidays.Select(e => e);
        }

        public IQueryable<Holiday> GetByDateRange(DateTime startRange, DateTime endRange)
        {
            if(startRange > endRange)
            {
                throw new ArgumentException("Start of period cannot be greater than end of period");
            }
            return _context.Holidays.Where(e => e.HolidayDate >= startRange && e.HolidayDate <= endRange);
        }

        private static void ValidateData(Holiday holiday)
        {
            if(holiday is null)
            {
                throw new NullReferenceException("Holiday cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(holiday.Description))
            {
                throw new ArgumentException("Description of holiday cannot be empty");
            }
            if(holiday.HolidayDate == DateTime.MinValue)
            {
                throw new ArgumentException("Holiday date cannot be empty");
            }
        }
    }
}
