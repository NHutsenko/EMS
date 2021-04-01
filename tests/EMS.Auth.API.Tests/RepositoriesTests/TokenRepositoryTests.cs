using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using EMS.Auth.API.DAL.Repositories;
using EMS.Auth.API.Models;
using Moq;
using NUnit.Framework;

namespace EMS.Auth.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class TokenRepositoryTests: BaseUnitTest<TokenRepository>
    {
        private UserToken _token;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            _token = new UserToken
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                AccessToken = Guid.NewGuid().ToString(),
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5),
                IsRefreshTokenExpired = false,
                RefreshToken = Guid.NewGuid().ToString(),
                UserId = 1
            };
            _dbContext.Tokens.Add(_token);
            _tokenRepository = new TokenRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void SaveTokenAsync_should_save_token_data_to_db()
        {
            // Arrange
            UserToken token = new()
            {
                AccessToken = Guid.NewGuid().ToString(),
                ExpiresIn = _dateTimeUtil.GetCurrentDateTime().AddMinutes(5),
                IsRefreshTokenExpired = false,
                RefreshToken = Guid.NewGuid().ToString(),
                UserId = 2
            };

            // Act
            int result = _tokenRepository.SaveTokenAsync(token).Result;

            // Assert
            Assert.AreEqual(1, result, "Saving result as expected");
            CollectionAssert.Contains(_dbContext.Tokens.ToList(), token, "Token added to db as expected");
            Assert.AreEqual(_dateTimeUtil.GetCurrentDateTime(), token.CreatedOn, "Creation date time as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DisableRefreshTokenAsync_should_disable_refresh_token_data()
        {
            // Act
            int result = _tokenRepository.DisableRefreshTokenAsync(_token.RefreshToken).Result;

            // Assert
            Assert.AreEqual(1, result, "Saving result as expected");
            Assert.IsTrue(_token.IsRefreshTokenExpired, "Expired refresh token param as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DisableRefreshTokenAsync_should_return_zero_because_token_data_by_refresh_token_not_found()
        {
            // Act
            int result = _tokenRepository.DisableRefreshTokenAsync(Guid.Empty.ToString()).Result;

            // Assert
            Assert.AreEqual(0, result, "Saving result as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void GetTokenData_should_retun_token_data_From_db_by_accessTiken()
        {
            // Act
            UserToken token = _tokenRepository.GetTokenData(_token.AccessToken);

            // Assert
            Assert.AreEqual(_token, token, "Token data as expected");
        }
    }
}
