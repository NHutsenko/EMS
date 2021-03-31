using System.Threading.Tasks;
using EMS.Auth.API.Models;

namespace EMS.Auth.API.Interfaces
{
    public interface IUsersRepository
    {
        Task<int> AddAsync(User user);
        Task<int> UpdateAsync(User user);
        Task<int> DeleteAsync(User user);
        User GetById(long id);
        User VerifyUser(string login, string password);
    }
}
