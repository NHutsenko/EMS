using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using Moq;

namespace EMS.Auth.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class JwtSecurityTokenHandlerMock
    {
        public static Mock<JwtSecurityTokenHandler> SetupMock()
        {
            Mock<JwtSecurityTokenHandler> mock = new();

            mock.Setup(m => m.WriteToken(It.IsAny<JwtSecurityToken>())).Returns<JwtSecurityToken>((token) =>
            {
                return Guid.Empty.ToString();
            });

            return mock;
        }
    }
}
