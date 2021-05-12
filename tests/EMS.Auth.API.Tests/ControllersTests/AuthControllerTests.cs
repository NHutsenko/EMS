using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using EMS.Auth.API.Controllers;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.RequestModels;
using EMS.Auth.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EMS.Auth.API.Tests.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class AuthControllerTests: BaseUnitTest<AuthService>
    {
        private User _user;
        private UserToken _token;

        [SetUp]
        public void Setup()
        {
            InitializeMocks(new AuthService(null, null, null, null, null));
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
            _authController = new AuthController(_authService);
        }

        [Test]
        public void AuthUserAsync_should_return_token_data_generated_by_auth_service()
        {
            // Arrange
            LoginUserRequest request = new()
            {
                Login = _user.Login,
                Password = _user.Password
            };
            TokenData expected = new()
            {
                IsSuccess = true,
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5),
                AccessToken = Guid.Empty.ToString(),
                RefreshToken = Guid.Empty.ToString(),
                ErrorMessage = string.Empty
            };


            // Act
            ObjectResult response = _authController.AuthUserAsync(request).Result as ObjectResult;
            TokenData actual = response.Value as TokenData;

            // Assert
            Assert.AreEqual(200, response.StatusCode, "StatusCode as expected");
            Assert.AreEqual(expected, actual, "Token data as expected");
            _authServiceMock.Verify(m => m.AuthUserAsync(request), Times.Once);
        }

        [Test]
        public void RefreshTokenAsync_should_return_new_token_data_via_auth_service()
        {
            // Arrange
            TokenData expected = new()
            {
                IsSuccess = true,
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5),
                AccessToken = Guid.Empty.ToString(),
                RefreshToken = Guid.Empty.ToString(),
                ErrorMessage = string.Empty
            };
            TokenData request = new()
            {
                AccessToken = _token.AccessToken,
                RefreshToken = _token.RefreshToken,
                ErrorMessage = string.Empty,
                ExpiresIn = _token.ExpiresIn,
                IsSuccess = true
            };

            // Act
            ObjectResult response = _authController.RefreshTokenAsync(request).Result as ObjectResult;
            TokenData actual = response.Value as TokenData;

            // Assert
            Assert.AreEqual(200, response.StatusCode, "StatusCode as expected");
            Assert.AreEqual(expected, actual, "Token data as expected");
            _authServiceMock.Verify(m => m.RefreshTokenAsync(request), Times.Once);
        }

        [Test]
        public void AuthUserAsync_should_have_allow_anonymous_attribute()
        {
            // Assert
            Type controllerType = typeof(AuthController);
            MethodBase methodBase = controllerType.GetMethod(nameof(_authController.AuthUserAsync));
            AllowAnonymousAttribute anonymousAttribute = methodBase.GetCustomAttribute<AllowAnonymousAttribute>();

            Assert.IsTrue(anonymousAttribute != null);
        }

        [Test]
        public void RefreshTokenAsync_should_have_authorize_attribute()
        {
            // Assert
            Type controllerType = typeof(AuthController);
            MethodBase methodBase = controllerType.GetMethod(nameof(_authController.RefreshTokenAsync));
            AuthorizeAttribute authorizeAttribute = methodBase.GetCustomAttribute<AuthorizeAttribute>();

            Assert.IsTrue(authorizeAttribute != null);
        }
    }
}
