using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using EMS.Auth.API.Controllers;
using EMS.Auth.API.Enums;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.ResponseModels;
using EMS.Auth.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EMS.Auth.API.Tests.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class UsersControllerTests : BaseUnitTest<UsersService>
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

            _userController = new UserController(_usersService);
            ControllerContext controllerContext = new()
            {
                HttpContext = _httpContext
            };
            _userController.ControllerContext = controllerContext;
        }

        [Test]
        public void AddUserAsync_should_have_authorize_attribute_with_role_admin()
        {
            // Assert
            Type controllerType = typeof(UserController);
            MethodBase methodBase = controllerType.GetMethod(nameof(_userController.AddUserAsync));
            AuthorizeAttribute authorizeAttribute = methodBase.GetCustomAttribute<AuthorizeAttribute>();

            Assert.IsTrue(authorizeAttribute != null);
            Assert.IsTrue(authorizeAttribute.Roles.Contains("Admin"));
        }

        [Test]
        public void AddUserAsync_should_add_user_via_users_service()
        {
            // Arrange
            User user = new()
            {
                Login = "Test1",
                Password = "test",
                Role = RoleType.HR
            };

            BaseResponse expected = new()
            {
                IsSucess = true,
                ErrorMessage = string.Empty,
                Id = 2
            };

            // Act
            ObjectResult result = _userController.AddUserAsync(user).Result as ObjectResult;
            BaseResponse actual = result.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, result.StatusCode, "StatusCode as expected");
            Assert.AreEqual(expected, actual, "Response data as expected");
            _usersServiceMock.Verify(m => m.AddAsync(user), Times.Once);
        }

        [Test]
        public void UpdateUserAsync_should_have_authorize_attribute()
        {
            // Assert
            Type controllerType = typeof(UserController);
            MethodBase methodBase = controllerType.GetMethod(nameof(_userController.UpdateUserAsync));
            AuthorizeAttribute authorizeAttribute = methodBase.GetCustomAttribute<AuthorizeAttribute>();

            Assert.IsTrue(authorizeAttribute != null);
        }

        [Test]
        public void UpdateUserAsync_should_update_user_via_users_service()
        {
            // Arrange
            _httpContext.Items["User"] = _user.Login;
            _user.Password = "test2";

            BaseResponse expected = new()
            {
                IsSucess = true,
                ErrorMessage = string.Empty,
                Id = 1
            };

            // Act
            ObjectResult result = _userController.UpdateUserAsync(_user).Result as ObjectResult;
            BaseResponse actual = result.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, result.StatusCode, "StatusCode as expected");
            Assert.AreEqual(expected, actual, "Response data as expected");
            _usersServiceMock.Verify(m => m.UpdateAsync(_user), Times.Once);
        }

        [Test]
        public void UpdateUserAsync_should_return_forbidden_because_authenticated_user_is_different_from_request_data()
        {
            // Arrange
            _httpContext.Items["User"] = "qwerty";
            _user.Password = "test2";

            // Act
            IActionResult result = _userController.UpdateUserAsync(_user).Result;

            // Assert
            Assert.That(result is ForbidResult);
            _usersServiceMock.Verify(m => m.UpdateAsync(_user), Times.Never);
        }

        [Test]
        public void DeleteUserAsync_should_have_authorize_attribute_with_role_admin()
        {
            // Assert
            Type controllerType = typeof(UserController);
            MethodBase methodBase = controllerType.GetMethod(nameof(_userController.DeleteUserAsync));
            AuthorizeAttribute authorizeAttribute = methodBase.GetCustomAttribute<AuthorizeAttribute>();

            Assert.IsTrue(authorizeAttribute != null);
            Assert.IsTrue(authorizeAttribute.Roles.Contains("Admin"));
        }

        [Test]
        public void DeleteUserAsync_should_return_succesfull_result_of_deleting_from_users_service()
        {
            // Arrange
            BaseResponse expected = new()
            {
                ErrorMessage = string.Empty,
                Id = _user.Id,
                IsSucess = true
            };

            // Act
            ObjectResult result = _userController.DeleteUserAsync(_user).Result as ObjectResult;
            BaseResponse actual = result.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, result.StatusCode, "StatusCode as expected");
            Assert.AreEqual(expected, actual, "Response as expected");
        }

        [Test]
        public void GetById_should_have_authorize_attribute()
        {
            // Assert
            Type controllerType = typeof(UserController);
            MethodBase methodBase = controllerType.GetMethod(nameof(_userController.GetUserById));
            AuthorizeAttribute authorizeAttribute = methodBase.GetCustomAttribute<AuthorizeAttribute>();

            Assert.IsTrue(authorizeAttribute != null);
        }

        [Test]
        public void GetById_should_return_response_with_user_data_by_id_from_users_service()
        {
            // Arrange
            UserResponse expected = new()
            {
                ErrorMessage = string.Empty,
                Id = _user.Id,
                IsSucess = true,
                User = _user
            };

            // Act
            ObjectResult result = _userController.GetUserById(_user.Id) as ObjectResult;
            UserResponse actual = result.Value as UserResponse;

            // Assert
            Assert.AreEqual(200, result.StatusCode, "Status Code as expected");
            Assert.AreEqual(expected, actual, "User data as expected");
            _usersServiceMock.Verify(m => m.GetById(_user.Id), Times.Once);
        }
    }
}
