using System.Threading.Tasks;
using EMS.Auth.API.Models.RequestModels;
using EMS.Auth.API.Models.ResponseModels;

namespace EMS.Auth.API.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> AuthUserAsync(LoginUserRequest request);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    }
}
