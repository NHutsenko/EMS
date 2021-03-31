using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using EMS.Auth.API.DAL.Repositories;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Tests.Mock;
using EMS.Auth.API.Tests.Mocks;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Moq;

namespace EMS.Auth.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class BaseUnitTest<T>
    {
        // Repos
        protected Mock<UsersRepository> _usersRepositoryMock;
        protected IUsersRepository _usersRepository;

        protected Mock<TokenRepository> _tokenRepositoryMock;
        protected ITokenRepository _tokenRepository;

        // DB context
        protected Mock<IApplicationDbContext> _dbContextMock;
        protected IApplicationDbContext _dbContext;

        // Logger
        protected Mock<IEMSLogger<T>> _loggerMock;
        protected IEMSLogger<T> _logger;

        // Utils
        protected IDateTimeUtil _dateTimeUtil;
        protected JwtSecurityTokenHandler _tokenHandler;

        public void InitializeMocks()
        {
            BaseMock.ShouldThrowException = false;

            _dateTimeUtil = new DateTimeUtilMock();
            _tokenHandler = JwtSecurityTokenHandlerMock.SetupMock().Object;

            DbContextMock.ShouldThrowException = false;
            DbContextMock.SaveChangesResult = 1;
            _dbContextMock = DbContextMock.SetupDbContext<IApplicationDbContext>();
            _dbContext = _dbContextMock.Object;

            _tokenRepositoryMock = TokenRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _tokenRepository = _tokenRepositoryMock.Object;

            _usersRepositoryMock = UsersRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _usersRepository = _usersRepositoryMock.Object;
        }

        public void InitializeLoggerMock(T loggerClass)
        {
            _loggerMock = LoggerMock.SetupMock(loggerClass);
            _logger = _loggerMock.Object;
        }
    }
}
