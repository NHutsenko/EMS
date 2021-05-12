using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.RequestModels;
using EMS.Auth.API.Services;
using EMS.Auth.API.Tests.Mock;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Moq;

namespace EMS.Auth.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class AuthServiceMock: BaseMock
    {
        public static Mock<AuthService> SetupMock(IUsersRepository usersRepository,
            ITokenRepository tokenRepository,
            IDateTimeUtil dateTimeUtil,
            IEMSLogger<AuthService> logger,
            JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            Mock<AuthService> mock = new(usersRepository, tokenRepository, dateTimeUtil, logger, jwtSecurityTokenHandler);
            AuthService service = new(usersRepository, tokenRepository, dateTimeUtil, logger, jwtSecurityTokenHandler);

            mock.Setup(m => m.AuthUserAsync(It.IsAny<LoginUserRequest>())).Returns<LoginUserRequest>((request) =>
            {
                return service.AuthUserAsync(request);
            });
            mock.Setup(m => m.RefreshTokenAsync(It.IsAny<TokenData>())).Returns<TokenData>((tokenData) =>
            {
                return service.RefreshTokenAsync(tokenData);
            });

            return mock;
        }
    }
}
