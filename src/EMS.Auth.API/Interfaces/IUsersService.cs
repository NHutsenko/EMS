using System.Threading.Tasks;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.ResponseModels;

namespace EMS.Auth.API.Interfaces
{
    public interface IUsersService
    {
        Task<BaseResponse> AddAsync(User user);
        Task<BaseResponse> DeleteAsync(User user);
        Task<BaseResponse> UpdateAsync(User user);
        UserResponse GetById(long id);
    }
}
