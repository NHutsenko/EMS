using System.Threading.Tasks;
using EMS.Auth.API.Models;

namespace EMS.Auth.API.Interfaces
{
    public interface ITokenRepository
    {
        Task<int> SaveTokenAsync(UserToken userToken);
        Task<int> DisableRefreshTokenAsync(string refreshToken);
        UserToken GetTokenData(string accessToken);
    }
}
