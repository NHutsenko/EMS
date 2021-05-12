using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using EMS.Auth.API.Controllers;
using EMS.Auth.API.DAL.Repositories;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Services;
using EMS.Auth.API.Tests.Mock;
using EMS.Auth.API.Tests.Mocks;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Http;
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

        //Services
        protected Mock<UsersService> _usersServiceMock;
        protected IUsersService _usersService;
        protected Mock<AuthService> _authServiceMock;
        protected IAuthService _authService;

        // Controllers
        protected AuthController _authController;
        protected UserController _userController;

        // DB context
        protected Mock<IApplicationDbContext> _dbContextMock;
        protected IApplicationDbContext _dbContext;

        // Logger
        protected Mock<IEMSLogger<T>> _loggerMock;
        protected IEMSLogger<T> _logger;

        // Utils
        protected IDateTimeUtil _dateTimeUtil;
        protected JwtSecurityTokenHandler _tokenHandler;

        // Context
        protected Mock<HttpContext> _httpContextMock;
        protected HttpContext _httpContext;

        public void InitializeMocks(T loggerClass)
        {
            _loggerMock = LoggerMock.SetupMock(loggerClass);
            _logger = _loggerMock.Object;
            BaseMock.ShouldThrowException = false;

            _httpContextMock = HttpContextMock.SetupHttpContextMock();
            _httpContext = _httpContextMock.Object;

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

            _authServiceMock = AuthServiceMock.SetupMock(_usersRepository, 
                _tokenRepository, 
                _dateTimeUtil, 
                _logger as IEMSLogger<AuthService>, 
                _tokenHandler);
            _authService = _authServiceMock.Object;

            _usersServiceMock = UsersServiceMock.SetupMock(_usersRepository,
                _logger as IEMSLogger<UsersService>, _dateTimeUtil);
            _usersService = _usersServiceMock.Object;
        }
    }
}
