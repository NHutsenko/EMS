using System;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.RequestModels;
using EMS.Auth.API.Models.ResponseModels;
using EMS.Auth.API.Services;
using EMS.Common.Logger.Models;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace EMS.Auth.API.Tests
{
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
            TokenResponse response = new()
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
            TokenResponse actual = _authService.AuthUserAsync(request).Result;

            // Assert
            Assert.AreEqual(response, actual, "Token as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _tokenRepositoryMock.Verify(m => m.SaveTokenAsync(token), Times.Once);
        }
    }
}
