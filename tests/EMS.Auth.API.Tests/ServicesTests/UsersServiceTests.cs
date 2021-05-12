using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Auth.API.Enums;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.ResponseModels;
using EMS.Auth.API.Services;
using EMS.Auth.API.Tests.Mock;
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
            InitializeMocks(new UsersService(null, null, null));
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
                ErrorMessage = $"Login cannot be empty"
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
            DbContextMock.ShouldThrowException = true;
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
                Response = new Exception(DbContextMock.ExceptionMessage)
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
            DbContextMock.SaveChangesResult = 0;
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
                ErrorMessage = $"User with login {user.Login} was not saved"
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
        public void UpdateAsync_should_succefully_update_user_into_db()
        {
            // Arrange
            _user.Role = RoleType.HR;
            BaseResponse expected = new()
            {
                Id = _user.Id,
                IsSucess = true,
                ErrorMessage = string.Empty
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user,
                Response = expected
            };


            // Act
            BaseResponse actual = _usersService.UpdateAsync(_user).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response from service as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _usersRepositoryMock.Verify(m => m.UpdateAsync(_user), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            _user.Role = RoleType.HR;
            _user.Password = string.Empty;
            BaseResponse expected = new()
            {
                IsSucess = false,
                ErrorMessage = "Password cannot be empty"
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user,
                Response = new Exception(expected.ErrorMessage)
            };


            // Act
            BaseResponse actual = _usersService.UpdateAsync(_user).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response from service as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _usersRepositoryMock.Verify(m => m.UpdateAsync(_user), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_db_update_exception()
        {
            // Arrange
            _user.Role = RoleType.HR;
            DbContextMock.ShouldThrowException = true;
            BaseResponse expected = new()
            {
                IsSucess = false,
                ErrorMessage = "An error occured while updating user"
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user,
                Response = new Exception(DbContextMock.ExceptionMessage)
            };


            // Act
            BaseResponse actual = _usersService.UpdateAsync(_user).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response from service as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _usersRepositoryMock.Verify(m => m.UpdateAsync(_user), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            _user.Role = RoleType.HR;
            DbContextMock.SaveChangesResult = 0;
            BaseResponse expected = new()
            {
                IsSucess = false,
                ErrorMessage = $"User with login {_user.Login} was not updated"
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user,
                Response = new Exception($"User with login {_user.Login} was not updated")
            };


            // Act
            BaseResponse actual = _usersService.UpdateAsync(_user).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response from service as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _usersRepositoryMock.Verify(m => m.UpdateAsync(_user), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_succefully_delete_user_from_db()
        {
            // Arrange
            BaseResponse expected = new()
            {
                Id = _user.Id,
                IsSucess = true,
                ErrorMessage = string.Empty
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user,
                Response = expected
            };


            // Act
            BaseResponse actual = _usersService.DeleteAsync(_user).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response from service as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _usersRepositoryMock.Verify(m => m.DeleteAsync(_user), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            BaseResponse expected = new()
            {
                IsSucess = false,
                ErrorMessage = "An error occured while deleting user"
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user,
                Response = new Exception(DbContextMock.ExceptionMessage)
            };


            // Act
            BaseResponse actual = _usersService.DeleteAsync(_user).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response from service as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _usersRepositoryMock.Verify(m => m.DeleteAsync(_user), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            BaseResponse expected = new()
            {
                IsSucess = false,
                ErrorMessage = $"User with login {_user.Login} was not deleted"
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user,
                Response = new Exception($"User with login {_user.Login} was not deleted")
            };


            // Act
            BaseResponse actual = _usersService.DeleteAsync(_user).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response from service as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _usersRepositoryMock.Verify(m => m.DeleteAsync(_user), Times.Once);
        }

        [Test]
        public void GetById_should_return_user_by_id()
        {
            // Arrage
            UserResponse expected = new()
            {
                Id = _user.Id,
                IsSucess = true,
                ErrorMessage = string.Empty,
                User = _user
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.GetById),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user.Id,
                Response = expected
            };

            // Act
            UserResponse actual = _usersService.GetById(_user.Id);

            // Assert
            Assert.AreEqual(expected, actual, "Response as expected");
            _usersRepositoryMock.Verify(m => m.GetById(_user.Id), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_handle_exception()
        {
            // Arrage
            BaseMock.ShouldThrowException = true;
            UserResponse expected = new()
            {
                Id = _user.Id,
                IsSucess = false,
                ErrorMessage = BaseMock.ExceptionMessage
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(UsersService),
                CallerMethodName = nameof(_usersService.GetById),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = _user.Id,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            UserResponse actual = _usersService.GetById(_user.Id);

            // Assert
            Assert.AreEqual(expected, actual, "Response as expected");
            _usersRepositoryMock.Verify(m => m.GetById(_user.Id), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
