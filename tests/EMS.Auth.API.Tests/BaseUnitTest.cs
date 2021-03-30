using System.Diagnostics.CodeAnalysis;
using EMS.Auth.API.DAL.Interfaces;
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
        protected Mock<IUsersRepository> _usersRepositoryMock;
        protected IUsersRepository _usersRepository;
        // DB context
        protected Mock<IApplicationDbContext> _dbContextMock;
        protected IApplicationDbContext _dbContext;

        // Logger
        protected Mock<IEMSLogger<T>> _loggerMock;
        protected IEMSLogger<T> _logger;

        // Utils
        protected IDateTimeUtil _dateTimeUtil;

        public void InitializeMocks()
        {
            BaseMock.ShouldThrowException = false;

            _dateTimeUtil = new DateTimeUtilMock();

            DbContextMock.ShouldThrowException = false;
            DbContextMock.SaveChangesResult = 1;
            _dbContextMock = DbContextMock.SetupDbContext<IApplicationDbContext>();
            _dbContext = _dbContextMock.Object;
        }

        public void InitializeLoggerMock(T loggerClass)
        {
            _loggerMock = LoggerMock.SetupMock(loggerClass);
            _logger = _loggerMock.Object;
        }
    }
}
