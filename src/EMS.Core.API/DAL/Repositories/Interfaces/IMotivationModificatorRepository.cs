using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories.Interfaces
{
    public interface IMotivationModificatorRepository
    {
        Task<int> AddAsync(MotivationModificator modificator);
        Task<int> UpdateAsync(MotivationModificator modificator);
        MotivationModificator GetByStaffId(long staffId);

    }
}
