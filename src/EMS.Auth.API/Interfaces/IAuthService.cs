using EMS.Auth.API.Models.ResponseModels;

namespace EMS.Auth.API.Interfaces
{
    public interface IAuthService
    {
        TokenResponse AuthUser(string login, string password);
        TokenResponse RefreshToken(string refreshToken);
    }
}
