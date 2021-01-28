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
            if(dayOff.PersonId == 0)
            {
                throw new ArgumentException("Cannot add day off record without specified person");
            }
            if(!IsRelevantTime(dayOff.Hours))
            {
                throw new ArgumentException("Cannot add day off record without specified time");
            }
            if(dayOff.CreatedOn == DateTime.MinValue)
            {
                throw new ArgumentException("Cannot add day off record without specified date");
            }
            if (_context.DaysOff.Any(e => e.CreatedOn.Date == dayOff.CreatedOn.Date && e.PersonId == dayOff.PersonId))
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
                throw new ArgumentException("Cannot update day off record without specified time");
            }
            if (dayOff.CreatedOn == DateTime.MinValue)
            {
                throw new ArgumentException("Cannot add day off record without specified date");
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

        public IQueryable<DayOff> GetByPersonId(long personId)
        {
            if(personId == 0)
            {
                throw new ArgumentException("Cannot get records for non existing person");
            }
            return _context.DaysOff.Where(e => e.PersonId == personId);
        }

        public IQueryable<DayOff> GetByDateRange(DateTime start, DateTime end)
        {
            if(end < start)
            {
                throw new ArgumentException("Wrong range date");
            }
            return _context.DaysOff.Where(e => e.CreatedOn >= start && e.CreatedOn <= end).OrderBy(e => e.CreatedOn);
        }

        public IQueryable<DayOff> GetByDateRangeAndPersonId(DateTime start, DateTime end, long personId)
        {
            if (personId == 0)
            {
                throw new ArgumentException("Cannot get records for non existing person");
            }
            return GetByDateRange(start, end).Where(e => e.PersonId == personId);
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
