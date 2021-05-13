using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using EMS.Auth.API.DAL.Repositories;
using EMS.Auth.API.Enums;
using EMS.Auth.API.Models;
using Moq;
using NUnit.Framework;

namespace EMS.Auth.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class UserRepositoryTests : BaseUnitTest<UsersRepository>
    {
        private User _user;

        [SetUp]
        public void Setup()
        {
            InitializeMocks(null);
            _user = new User
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Login = "Test",
                Password = "test",
                Role = RoleType.Admin
            };
            _dbContext.Users.Add(_user);
            _usersRepository = new UsersRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void AddAsync_should_add_user_to_db()
        {
            // Arrange
            User user = new()
            {
                Login = "testtest",
                Password = "1234",
                Role = RoleType.Employee
            };

            // Act
            int result = _usersRepository.AddAsync(user).Result;

            // Assert
            Assert.AreEqual(1, result, "SaveChangesResult as expected");
            CollectionAssert.Contains(_dbContext.Users.ToList(), user, "Inserted as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_argument_exception_because_login_is_empty()
        {
            // Arrange
            // Arrange
            User user = new()
            {
                Login = string.Empty,
                Password = "1234",
                Role = RoleType.Employee
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _usersRepository.AddAsync(user), "Throws argument exception as expected");
        }

        [Test]
        public void AddAsync_should_throw_argument_exception_because_password_is_empty()
        {
            // Arrange
            User user = new()
            {
                Login = "testtest",
                Password = string.Empty,
                Role = RoleType.Employee
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _usersRepository.AddAsync(user), "Throws argument exception as expected");
        }

        [Test]
        public void AddAsync_should_throw_invalid_operation_exception_because_user_with_login_already_in_db()
        {
            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _usersRepository.AddAsync(_user), "Throws argument exception as expected");
        }

        [Test]
        public void UpdateAsync_should_update_user_into_db()
        {
            // Arrange
            User user = new()
            {
                Id = 1,
                Login = "testtest",
                Password = "1234",
                Role = RoleType.Employee
            };

            // Act
            int result = _usersRepository.UpdateAsync(user).Result;

            // Assert
            Assert.AreEqual(1, result, "SaveChangesResult as expected");
            CollectionAssert.Contains(_dbContext.Users.ToList(), user, "User saved via context as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_argument_exception_because_login_is_empty()
        {
            // Arrange
            User user = new()
            {
                Id = 1,
                Login = string.Empty,
                Password = "1234",
                Role = RoleType.Employee
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _usersRepository.UpdateAsync(user), "Throws argument exception as expected");
        }

        [Test]
        public void UpdateAsync_should_throw_argument_exception_because_password_is_empty()
        {
            // Arrange
            User user = new()
            {
                Id = 1,
                Login = "testtest",
                Password = string.Empty,
                Role = RoleType.Employee
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _usersRepository.UpdateAsync(user), "Throws argument exception as expected");
        }

        [Test]
        public void DeleteAsync_should_remove_entity_from_db()
        {
            // Act
            int result = _usersRepository.DeleteAsync(_user).Result;

            // Assert
            Assert.AreEqual(1, result, "Result as expected");
            CollectionAssert.DoesNotContain(_dbContext.Users.ToList(), _user, "Removed from DB as expected");
        }

        [Test]
        public void VerifyUser_should_return_user_from_db()
        {
            // Act
            User actual = _usersRepository.VerifyUser(_user.Login, _user.Password);

            // Assert
            Assert.AreEqual(_user, actual, "Returned user as expected");
        }

        [Test]
        public void GetById_should_return_user_entity_by_id()
        {
            // Act
            User actual = _usersRepository.GetById(_user.Id);

            // Assert
            Assert.AreEqual(_user, actual, "Returned user as expected");
        }

        [Test]
        public void GetByLogin_should_return_user_entity_by_login()
        {
            // Act
            User actual = _usersRepository.GetByLogin(_user.Login);

            // Assert
            Assert.AreEqual(_user, actual, "Returned user as expected");
        }
    }
}
