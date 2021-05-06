using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Auth.API.Enums;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.ResponseModels;
using EMS.Auth.API.Services;
using EMS.Common.Logger.Models;
using Moq;
using NUnit.Framework;

namespace EMS.Auth.API.Tests.ServicesTests
{
    [ExcludeFromCodeCoverage]
    public class UsersServiceTests : BaseUnitTest<UsersService>
    {
        private User _user;
        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            _user = new User
            {
                Id = 1,
                Login = "test",
                Password = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Role = RoleType.Admin
            };
            _dbContext.Users.Add(_user);

            _usersService = new UsersService(_usersRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetById_should_return_user_by_id_from_db()
        {
            // Act
            User actual = _usersService.GetById(_user.Id);

            // Assert
            Assert.AreEqual(_user, actual, "Returned user by id as expected");
        }

        public 

        [Test]
        public void AddAsync_should_return_success_result_of_adding_user()
        {
            // Arrange
            User user = new()
            {
                Login = "Test2",
                Password = "Test2",
                Role = RoleType.Admin
            };

            BaseResponse expectedResponse = new()
            {
                Id = 2,
                IsSucess = true,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = user,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _usersService.AddAsync(user).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _usersRepositoryMock.Verify(m => m.AddAsync(user), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_return_message_that_user_is_already_exists()
        {
            // Arrange
            User user = new()
            {
                Login = "test",
                Password = "Test2",
                Role = RoleType.Admin
            };

            BaseResponse expectedResponse = new()
            {
                Id = 0,
                IsSucess = false,
                ErrorMessage = $"User with login '{user.Login}' already exists"
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = user,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _usersService.AddAsync(user).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _usersRepositoryMock.Verify(m => m.AddAsync(user), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_return_message_that_user_login_is_empty()
        {
            // Arrange
            User user = new()
            {
                Login = string.Empty,
                Password = "Test2",
                Role = RoleType.Admin
            };

            BaseResponse expectedResponse = new()
            {
                Id = 0,
                IsSucess = false,
                ErrorMessage = $"User login cannot be empty"
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = user,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _usersService.AddAsync(user).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _usersRepositoryMock.Verify(m => m.AddAsync(user), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_return_message_db_update_exception()
        {
            // Arrange
            User user = new()
            {
                Login = "Test2",
                Password = "Test2",
                Role = RoleType.Admin
            };

            BaseResponse expectedResponse = new()
            {
                Id = 0,
                IsSucess = false,
                ErrorMessage = $"An error occured while saving user"
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = user,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _usersService.AddAsync(user).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _usersRepositoryMock.Verify(m => m.AddAsync(user), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_return_message_that_user_has_not_saved()
        {
            // Arrange
            User user = new()
            {
                Login = "Test2",
                Password = "Test2",
                Role = RoleType.Admin
            };

            BaseResponse expectedResponse = new()
            {
                Id = 0,
                IsSucess = false,
                ErrorMessage = $"User with login {user.Login} has not saved"
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = user,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _usersService.AddAsync(user).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _usersRepositoryMock.Verify(m => m.AddAsync(user), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
