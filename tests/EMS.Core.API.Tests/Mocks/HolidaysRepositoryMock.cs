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
    public class HolidaysRepositoryMock: BaseMock
    {

        public static Mock<HolidaysRepository> SetupMock(IApplicationDbContext dbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<HolidaysRepository> mock = new(dbContext, dateTimeUtil);
            HolidaysRepository repository = new(dbContext, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<Holiday>())).Returns<Holiday>((holiday) =>
            {
                return repository.AddAsync(holiday);
            });

            mock.Setup(m => m.DeleteAsync(It.IsAny<Holiday>())).Returns<Holiday>((holiday) =>
            {
                return repository.DeleteAsync(holiday);
            });


            mock.Setup(m => m.UpdateAsync(It.IsAny<Holiday>())).Returns<Holiday>((holiday) =>
            {
                return repository.UpdateAsync(holiday);
            });

            mock.Setup(m => m.GetAll()).Returns(() =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetAll();
            });

            mock.Setup(m => m.GetByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns<DateTime, DateTime>((start, end) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetByDateRange(start, end);
            });

            return mock;
        }
    }
}
