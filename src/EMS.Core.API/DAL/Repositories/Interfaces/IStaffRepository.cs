using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories.Interfaces
{
    public interface IStaffRepository
    {
        Task<int> AddAsync(Staff staff);
        Task<int> UpdateAsync(Staff staff);
        Task<int> DeleteAsync(Staff staff);
        public IQueryable<Staff> GetByPersonId(long personId);
        public IQueryable<Staff> GetByManagerId(long managerId);
        public IQueryable<Staff> GetAll();
    }
}
