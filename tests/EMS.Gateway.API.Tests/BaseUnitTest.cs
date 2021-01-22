using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Services;
using EMS.Core.API.Tests.Mock;
using Microsoft.Extensions.Logging;
using Moq;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class BaseUnitTest
    {
        // Context
        protected Mock<IApplicationDbContext> _dbContextMock;
        protected IApplicationDbContext _dbContext;

        // Utils
        protected IDateTimeUtil _dateTimeUtil;

        // Repos
        protected DayOffRepository _dayOffRepository;
        protected PeopleRepository _peopleRepository;
        protected PositionsRepository _positionsRepository;
        protected StaffRepository _staffRepository;
        protected TeamsRepository _teamsRepository;
        protected OtherPaymentsRepository _otherPaymentsRepository;
        protected HolidaysRepository _holidaysRepository;
        protected MotivationModificatorRepository _motivationModificatorRepository;

        // Services
        protected SalaryService _salaryService;
        protected PeopleService _peopleService;
        protected TeamsService _teamsService;

        protected void InitializeMocks()
        {
            // DB context
            _dbContextMock = DbContextMock.SetupDbContext<IApplicationDbContext>();
            _dbContext = _dbContextMock.Object;

            // Utils
            _dateTimeUtil = new DateTimeUtilMock();
        }
    }
}
