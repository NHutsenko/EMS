using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Moq;

namespace EMS.Core.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class DayOffsRepositoryMock: BaseMock
    {
        public static Mock<DayOffRepository> SetupMock(IApplicationDbContext dbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<DayOffRepository> mock = new Mock<DayOffRepository>(dbContext, dateTimeUtil);
            DayOffRepository repository = new DayOffRepository(dbContext, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<DayOff>())).Returns<DayOff>((dayOff) =>
            {
                return repository.AddAsync(dayOff);
            });

            mock.Setup(m => m.UpdateAsync(It.IsAny<DayOff>())).Returns<DayOff>((dayOff) =>
            {
                return repository.UpdateAsync(dayOff);
            });

            mock.Setup(m => m.DeleteAsync(It.IsAny<DayOff>())).Returns<DayOff>((dayOff) =>
            {
                return repository.DeleteAsync(dayOff);
            });

            mock.Setup(m => m.GetByPersonId(It.IsAny<long>())).Returns<long>((id) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetByPersonId(id);
            });

            mock.Setup(m => m.GetByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns<DateTime, DateTime>((start, end) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetByDateRange(start, end);
            });

            mock.Setup(m => m.GetByDateRangeAndPersonId(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<long>())).Returns<DateTime, DateTime, long>((start, end, id) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetByDateRangeAndPersonId(start, end, id);
            });

            return mock;
        }
    }
}
