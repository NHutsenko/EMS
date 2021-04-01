using System.Threading.Tasks;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.RequestModels;

namespace EMS.Auth.API.Interfaces
{
    public interface IAuthService
    {
        Task<TokenData> AuthUserAsync(LoginUserRequest request);
        Task<TokenData> RefreshTokenAsync(TokenData toRefresh);
    }
}
