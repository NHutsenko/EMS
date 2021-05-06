using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.RequestModels;
using EMS.Auth.API.Services;
using EMS.Auth.API.Tests.Mock;
using EMS.Common.Logger.Models;
using Moq;

using NUnit.Framework;

namespace EMS.Auth.API.Tests.ServicesTests
{
    [ExcludeFromCodeCoverage]
    public class AuthServiceTests: BaseUnitTest<AuthService>
    {
        private User _user;
        private UserToken _token;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new AuthService(null, null, null, null, null));
            _user = new User
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Login = "test",
                Password = "test",
                Role = Enums.RoleType.Admin
            };
            _dbContext.Users.Add(_user);

            _token = new UserToken
            {
                Id = 1,
                AccessToken = Guid.Empty.ToString(),
                RefreshToken = Guid.Empty.ToString(),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5),
                IsRefreshTokenExpired = false,
                UserId = _user.Id
            };
            _dbContext.Tokens.Add(_token);
            _authService = new AuthService(_usersRepository, _tokenRepository, _dateTimeUtil, _logger, _tokenHandler);
        }

        [Test]
        public void AuthUserAsync_should_return_refresh_and_access_token_with_saving_them_to_db()
        {
            // Arrange
            LoginUserRequest request = new()
            {
                Login = _user.Login,
                Password = _user.Password
            };
            TokenData response = new()
            {
                AccessToken = Guid.Empty.ToString(),
                RefreshToken = Guid.Empty.ToString(),
                ErrorMessage = string.Empty,
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5),
                IsSuccess = true
            };

            UserToken token = new()
            {
                Id = 2,
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5),
                UserId = _user.Id,
                IsRefreshTokenExpired = false
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(AuthService),
                CallerMethodName = nameof(_authService.AuthUserAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            TokenData actual = _authService.AuthUserAsync(request).Result;

            // Assert
            Assert.AreEqual(response, actual, "Token as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _tokenRepositoryMock.Verify(m => m.SaveTokenAsync(token), Times.Once);
        }

        [Test]
        public void AuthUserAsync_should_return_error_that_user_login_data_is_wrong()
        {
            // Arrange
            LoginUserRequest request = new()
            {
                Login = "wrong",
                Password = "wrong"
            };
            TokenData response = new()
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ErrorMessage = "Invalid login or password",
                IsSuccess = false
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(AuthService),
                CallerMethodName = nameof(_authService.AuthUserAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Invalid login or password")
            };

            // Act
            TokenData actual = _authService.AuthUserAsync(request).Result;

            // Assert
            Assert.AreEqual(response, actual, "Token as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AuthUserAsync_should_return_error_because_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            LoginUserRequest request = new()
            {
                Login = _user.Login,
                Password = _user.Password
            };
            TokenData response = new()
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ErrorMessage = $"An error occured while authentcating user {_user.Login}",
                IsSuccess = false
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(AuthService),
                CallerMethodName = nameof(_authService.AuthUserAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(DbContextMock.ExceptionMessage)
            };

            // Act
            TokenData actual = _authService.AuthUserAsync(request).Result;

            // Assert
            Assert.AreEqual(response, actual, "Token as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AuthUserAsync_should_return_error_because_saving_result_is_0()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            LoginUserRequest request = new()
            {
                Login = _user.Login,
                Password = _user.Password
            };
            TokenData response = new()
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ErrorMessage = $"An error occured while authentcating user {_user.Login}",
                IsSuccess = false
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(AuthService),
                CallerMethodName = nameof(_authService.AuthUserAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("An error occured while saving token data")
            };

            // Act
            TokenData actual = _authService.AuthUserAsync(request).Result;

            // Assert
            Assert.AreEqual(response, actual, "Token as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void RefreshTokenAsync_should_refresh_token_and_return_new_token_data()
        {
            // Arrange
            TokenData request = new()
            {
                AccessToken = _token.AccessToken,
                RefreshToken = _token.RefreshToken,
                ErrorMessage = string.Empty,
                IsSuccess = true,
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5)
            };
            UserToken token = new()
            {
                Id = 2,
                AccessToken = _token.AccessToken,
                RefreshToken = _token.RefreshToken,
                UserId = _token.UserId,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5),
                IsRefreshTokenExpired = false
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(AuthService),
                CallerMethodName = nameof(_authService.RefreshTokenAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = request
            };

            // Act
            TokenData tokenData = _authService.RefreshTokenAsync(request).Result;

            // Assert
            Assert.AreEqual(request, tokenData, "TokenData as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _tokenRepositoryMock.Verify(m => m.DisableRefreshTokenAsync(request.RefreshToken), Times.Once);
            _tokenRepositoryMock.Verify(m => m.SaveTokenAsync(token), Times.Once);
        }

        [Test]
        public void RefreshTokenAsync_should_return_error_because_access_token_does_not_exists_in_db()
        {
            // Arrange
            TokenData request = new()
            {
                AccessToken = "broken",
                RefreshToken = _token.RefreshToken,
                ErrorMessage = string.Empty,
                IsSuccess = true,
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5)
            };
            TokenData response = new()
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ErrorMessage = "Authentication failed",
                IsSuccess = false
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(AuthService),
                CallerMethodName = nameof(_authService.RefreshTokenAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception($"Token data for token {request.AccessToken} does mot exists into DB")
            };

            // Act
            TokenData tokenData = _authService.RefreshTokenAsync(request).Result;

            // Assert
            Assert.AreEqual(response, tokenData, "TokenData as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void RefreshTokenAsync_should_return_error_because_db_update_exception_for_new_token()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            TokenData request = new()
            {
                AccessToken = _token.AccessToken,
                RefreshToken = _token.RefreshToken,
                ErrorMessage = string.Empty,
                IsSuccess = true,
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5)
            };
            TokenData response = new()
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ErrorMessage = "Authentication failed",
                IsSuccess = false
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(AuthService),
                CallerMethodName = nameof(_authService.RefreshTokenAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(DbContextMock.ExceptionMessage)
            };

            // Act
            TokenData tokenData = _authService.RefreshTokenAsync(request).Result;

            // Assert
            Assert.AreEqual(response, tokenData, "TokenData as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void RefreshTokenAsync_should_return_error_because_new_token_has_not_been_saved()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            TokenData request = new()
            {
                AccessToken = _token.AccessToken,
                RefreshToken = _token.RefreshToken,
                ErrorMessage = string.Empty,
                IsSuccess = true,
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5)
            };
            TokenData response = new()
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ErrorMessage = "Authentication failed",
                IsSuccess = false
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(AuthService),
                CallerMethodName = nameof(_authService.RefreshTokenAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("An error occured while saving new access token")
            };

            // Act
            TokenData tokenData = _authService.RefreshTokenAsync(request).Result;

            // Assert
            Assert.AreEqual(response, tokenData, "TokenData as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
