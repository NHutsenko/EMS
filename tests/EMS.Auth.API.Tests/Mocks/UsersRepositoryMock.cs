using System.Diagnostics.CodeAnalysis;
using EMS.Auth.API.DAL.Repositories;
using EMS.Auth.API.Interfaces;
using EMS.Common.Utils.DateTimeUtil;
using Moq;

namespace EMS.Auth.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class UsersRepositoryMock: BaseMock
    {
        public static Mock<UsersRepository> SetupMock(IApplicationDbContext applicationDbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<UsersRepository> mock = new(applicationDbContext, dateTimeUtil);
            UsersRepository repository = new(applicationDbContext, dateTimeUtil);

            mock.Setup(m => m.VerifyUser(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((login, password) =>
            {
                ThrowExceptionIfNeeded();
                return repository.VerifyUser(login, password);
            });

            return mock;
        }
    }
}
