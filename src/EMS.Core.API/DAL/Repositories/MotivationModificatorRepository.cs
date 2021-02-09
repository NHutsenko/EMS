using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories
{
    public class MotivationModificatorRepository : BaseRepository, IMotivationModificatorRepository
    {
        public MotivationModificatorRepository(IApplicationDbContext context, IDateTimeUtil dateTimeUtil) : base(context, dateTimeUtil) { }

        public async Task<int> AddAsync(MotivationModificator modificator)
        {
            CheckData(modificator);
            if(_context.MotivationModificators.Any(e => e.StaffId == modificator.StaffId))
            {
                throw new InvalidOperationException("Motivation modificator already exists for specified work period");
            }
            modificator.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.MotivationModificators.Add(modificator);
            return await _context.SaveChangesAsync();
        }

        public MotivationModificator GetByStaffId(long staffId)
        {
            return _context.MotivationModificators.FirstOrDefault(e => e.StaffId == staffId);
        }

        public async Task<int> UpdateAsync(MotivationModificator modificator)
        {
            CheckData(modificator);
            _context.MotivationModificators.Update(modificator);
            return await _context.SaveChangesAsync();
        }

        private void CheckData(MotivationModificator modificator)
        {
            if (modificator is null)
            {
                throw new NullReferenceException("Motificator cannot be empty");
            }
            if(modificator.StaffId == 0)
            {
                throw new ArgumentException("Work period is not specified");
            }
            if(!_context.Staff.Any(e => e.Id == modificator.StaffId))
            {
                throw new ArgumentException("Cannot modify motivation for work period which does not exists");
            }
            if(modificator.ModValue <= 0)
            {
                throw new ArgumentException("Motivation mod cannot be less than 0 or equal to 0");
            }
        }
    }
}
