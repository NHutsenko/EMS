using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EMS.Core.API.DAL.Repositories
{
    public class DayOffRepository : BaseRepository, IDayOffRepository
    {
        public DayOffRepository(IApplicationDbContext context) : base(context, null) { }

        public async Task<int> AddAsync(DayOff dayOff)
        {
            if(dayOff is null)
            {
                throw new NullReferenceException("Dayoff entity cannot be null");
            }
            if(dayOff.StaffId == 0)
            {
                throw new DbUpdateException("Cannot add day off record without specified staff Id");
            }
            if(!IsRelevantTime(dayOff.Hours))
            {
                throw new DbUpdateException("Cannot add day off record without specified time");
            }
            if(dayOff.CreatedOn == DateTime.MinValue)
            {
                throw new DbUpdateException("Cannot add day off record without specified date");
            }
            if (_context.DaysOff.Any(e => e.CreatedOn.Date == dayOff.CreatedOn.Date && e.StaffId == dayOff.StaffId))
            {
                return 0;
            }
            _context.DaysOff.Add(dayOff);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(DayOff dayOff)
        {
            if (dayOff is null)
            {
                throw new NullReferenceException("Dayoff entity cannot be null");
            }
            if (!IsRelevantTime(dayOff.Hours))
            {
                throw new DbUpdateException("Cannot update day off record without specified time");
            }
            if (dayOff.CreatedOn == DateTime.MinValue)
            {
                throw new DbUpdateException("Cannot add day off record without specified date");
            }
            _context.DaysOff.Update(dayOff);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(DayOff dayOff)
        {
            _context.DaysOff.Remove(dayOff);
            return await _context.SaveChangesAsync();
        }

        public IQueryable<DayOff> GetAll()
        {
            return _context.DaysOff.Select(e => e);
        }

        public IQueryable<DayOff> GetByStaffId(long staffId)
        {
            if(staffId == 0)
            {
                throw new ArgumentException("Cannot get records for staff Id equals to 0");
            }
            return _context.DaysOff.Where(e => e.StaffId == staffId);
        }

        public IQueryable<DayOff> GetByDateRange(DateTime start, DateTime end)
        {
            if(end < start)
            {
                throw new ArgumentException("Wrong range date");
            }
            return _context.DaysOff.Where(e => e.CreatedOn >= start && e.CreatedOn <= end).OrderBy(e => e.CreatedOn);
        }

        public IQueryable<DayOff> GetByDateRangeAndStaffId(DateTime start, DateTime end, long staffId)
        {
            if (staffId == 0)
            {
                throw new ArgumentException("Cannot get records for staff Id equals to 0");
            }
            return GetByDateRange(start, end).Where(e => e.StaffId == staffId);
        }

        [ExcludeFromCodeCoverage]
        private static bool IsRelevantTime(float hours)
        {
            bool parsed = float.TryParse(Environment.GetEnvironmentVariable("MaxWorkHours"), out float maxVlaue);
            if (!parsed)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                _ = float.TryParse(configuration["Settings:MaxWorkHours"], out maxVlaue);
            }
            return hours > 0 && hours <= maxVlaue;
        }
    }
}
