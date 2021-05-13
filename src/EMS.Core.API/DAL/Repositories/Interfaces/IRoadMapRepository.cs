using System.Threading.Tasks;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories.Interfaces
{
    public interface IRoadMapRepository
    {
        Task<int> AddAsync(RoadMap roadMap);
        Task<int> UpdateAsync(RoadMap roadMap);
        Task<int> DeleteAsync(RoadMap roadMap);
        RoadMap GetByStaffId(long staffId);
    }
}
