using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Services;
using EMS.Core.API.Tests.Mock;
using EMS.Core.API.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class BaseUnitTest<T>
    {
        // Context
        protected Mock<IApplicationDbContext> _dbContextMock;
        protected IApplicationDbContext _dbContext;

        // Utils
        protected IDateTimeUtil _dateTimeUtil;

        // Repos
        protected Mock<DayOffRepository> _dayOffRepositoryMock;
        protected DayOffRepository _dayOffRepository;

        protected Mock<PeopleRepository> _peopleRepositoryMock;
        protected PeopleRepository _peopleRepository;

        protected Mock<PositionsRepository> _positionsRepositoryMock;
        protected PositionsRepository _positionsRepository;

        protected Mock<StaffRepository> _staffRepositoryMock;
        protected StaffRepository _staffRepository;

        protected Mock<TeamsRepository> _teamsRepositoryMock;
        protected TeamsRepository _teamsRepository;

        protected Mock<OtherPaymentsRepository> _otherPaymentsRepositoryMock;
        protected OtherPaymentsRepository _otherPaymentsRepository;

        protected Mock<HolidaysRepository> _holidaysRepositoryMock;
        protected HolidaysRepository _holidaysRepository;

        protected Mock<MotivationModificatorRepository> _motivationModificatorRepositoryMock;
        protected MotivationModificatorRepository _motivationModificatorRepository;

        // Services
        protected SalaryService _salaryService;
        protected PeopleService _peopleService;
        protected TeamsService _teamsService;
        protected PositionsService _positionsService;
        protected HolidaysService _holidaysService;
        protected DayOffsService _dayOffsService;
        protected OtherPaymentsService _otherPaymentsService;
        protected MotivationModificatorsService _motivationModificatorsService;
        protected StaffService _staffService;

        // Logger
        protected Mock<IEMSLogger<T>> _loggerMock;
        protected IEMSLogger<T> _logger;

        protected void InitializeMocks()
        {
            // DB context
            DbContextMock.ShouldThrowException = false;
            DbContextMock.SaveChangesResult = 1;
            _dbContextMock = DbContextMock.SetupDbContext<IApplicationDbContext>();
            _dbContext = _dbContextMock.Object;

            // Utils
            _dateTimeUtil = new DateTimeUtilMock();

            // Repositories
            BaseMock.ShouldThrowException = false;

            _teamsRepositoryMock = TeamsRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _teamsRepository = _teamsRepositoryMock.Object;

            _positionsRepositoryMock = PositionsRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _positionsRepository = _positionsRepositoryMock.Object;

            _dayOffRepositoryMock = DayOffsRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _dayOffRepository = _dayOffRepositoryMock.Object;

            _holidaysRepositoryMock = HolidaysRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _holidaysRepository = _holidaysRepositoryMock.Object;

            _motivationModificatorRepositoryMock = MotivationModificatorRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _motivationModificatorRepository = _motivationModificatorRepositoryMock.Object;

            _otherPaymentsRepositoryMock = OtherPaymentsRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _otherPaymentsRepository = _otherPaymentsRepositoryMock.Object;

            _peopleRepositoryMock = PeopleRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _peopleRepository = _peopleRepositoryMock.Object;

            _staffRepositoryMock = StaffRepositoryMock.SetupMock(_dbContext, _dateTimeUtil);
            _staffRepository = _staffRepositoryMock.Object;
        }

        protected void InitializeLoggerMock(T loggerClass)
        {
            _loggerMock = LoggerMock.SetupMock(loggerClass);
            _logger = _loggerMock.Object;
        }
    }
}
