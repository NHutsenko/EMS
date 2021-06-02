using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories
{
    public class RoadMapRepository : BaseRepository, IRoadMapRepository
    {
        public RoadMapRepository(IApplicationDbContext applicationDbContext, IDateTimeUtil dateTimeUtil) : base(applicationDbContext, dateTimeUtil) { }

        public virtual async Task<int> AddAsync(RoadMap roadMap)
        {
            if(!_context.Staff.Any(e => e.Id == roadMap.StaffId))
            {
                throw new ArgumentException("Cannot create road map for non-existent work period");
            }
            if(_context.Staff.FirstOrDefault(e => e.Id == roadMap.StaffId).RoadMapId != 0)
            {
                throw new InvalidOperationException("Road map already exists");
            }
            if (string.IsNullOrWhiteSpace(roadMap.Tasks))
            {
                throw new ArgumentException("Tasks for road map was not provided");
            }
            roadMap.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.RoadMaps.Add(roadMap);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(RoadMap roadMap)
        {
            _context.RoadMaps.Remove(roadMap);
            return await _context.SaveChangesAsync();
        }

        public virtual RoadMap GetByStaffId(long staffId)
        {
            return _context.RoadMaps.FirstOrDefault(e => e.StaffId == staffId);
        }

        public virtual async Task<int> UpdateAsync(RoadMap roadMap)
        {
            if (string.IsNullOrWhiteSpace(roadMap.Tasks))
            {
                throw new ArgumentException("Tasks for road map was not provided");
            }
            roadMap.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.RoadMaps.Update(roadMap);
            return await _context.SaveChangesAsync();
        }
    }
}
