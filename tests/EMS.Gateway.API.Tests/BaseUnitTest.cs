using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Tests.Mocks;
using Moq;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
	public class BaseUnitTest
	{
		protected Mock<IApplicationDbContext> _dbContextMock;
		protected IApplicationDbContext _dbContext;
        protected IDateTimeUtil _dateTimeUtil;

        protected DayOffRepository _dayOffRepository;
        protected PeopleRepository _peopleRepository;
        protected PositionsRepository _positionsRepository;
        protected StaffRepository _staffRepository;
        protected TeamsRepository _teamsRepository;
        protected OtherPaymentsRepository _otherPaymentsRepository;

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
