using System.Diagnostics.CodeAnalysis;
using EMS.Auth.API.DAL.Repositories;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Common.Utils.DateTimeUtil;
using Moq;

namespace EMS.Auth.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class TokenRepositoryMock: BaseMock
    {
        public static Mock<TokenRepository> SetupMock(IApplicationDbContext applicationDbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<TokenRepository> mock = new(applicationDbContext, dateTimeUtil);
            TokenRepository repository = new(applicationDbContext, dateTimeUtil);

            mock.Setup(m => m.SaveTokenAsync(It.IsAny<UserToken>())).Returns<UserToken>((token) =>
            {
                ThrowExceptionIfNeeded();
                return repository.SaveTokenAsync(token);
            });

            mock.Setup(m => m.DisableRefreshTokenAsync(It.IsAny<string>())).Returns<string>((token) =>
            {
                ThrowExceptionIfNeeded();
                return repository.DisableRefreshTokenAsync(token);
            });

            return mock;
        }
    }
}
