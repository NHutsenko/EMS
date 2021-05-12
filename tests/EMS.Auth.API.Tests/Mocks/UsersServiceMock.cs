using System.Diagnostics.CodeAnalysis;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Services;
using EMS.Auth.API.Tests.Mock;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Moq;

namespace EMS.Auth.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class UsersServiceMock: BaseMock
    {
        public static Mock<UsersService> SetupMock(IUsersRepository usersRepository, 
            IEMSLogger<UsersService> logger, 
            IDateTimeUtil dateTimeUtil)
        {
            Mock<UsersService> mock = new(usersRepository, logger, dateTimeUtil);
            UsersService service = new(usersRepository, logger, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<User>())).Returns<User>((user) =>
            {
                return service.AddAsync(user);
            });

            mock.Setup(m => m.UpdateAsync(It.IsAny<User>())).Returns<User>((user) =>
            {
                return service.UpdateAsync(user);
            });

            mock.Setup(m => m.DeleteAsync(It.IsAny<User>())).Returns<User>((user) =>
            {
                return service.DeleteAsync(user);
            });

            mock.Setup(m => m.GetById(It.IsAny<long>())).Returns<long>((id) =>
            {
                return service.GetById(id);
            });

            return mock;
        }
    }
}
