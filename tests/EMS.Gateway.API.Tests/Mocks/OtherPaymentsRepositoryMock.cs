using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Moq;

namespace EMS.Core.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class OtherPaymentsRepositoryMock : BaseMock
    {
        public static Mock<OtherPaymentsRepository> SetupMock(IApplicationDbContext dbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<OtherPaymentsRepository> mock = new Mock<OtherPaymentsRepository>(dbContext, dateTimeUtil);
            OtherPaymentsRepository repository = new OtherPaymentsRepository(dbContext, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<OtherPayment>())).Returns<OtherPayment>((payment) =>
            {
                return repository.AddAsync(payment);
            });

            mock.Setup(m => m.UpdateAsync(It.IsAny<OtherPayment>())).Returns<OtherPayment>((payment) =>
            {
                return repository.UpdateAsync(payment);
            });

            mock.Setup(m => m.DeleteAsync(It.IsAny<OtherPayment>())).Returns<OtherPayment>((payment) =>
            {
                return repository.DeleteAsync(payment);
            });

            mock.Setup(m => m.GetByPersonId(It.IsAny<long>())).Returns<long>((personId) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetByPersonId(personId);
            });

            mock.Setup(m => m.GetByPersonIdAndDateRange(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns<long, DateTime, DateTime>(
                (personId, start, end) =>
             {
                 ThrowExceptionIfNeeded();
                 return repository.GetByPersonIdAndDateRange(personId, start, end);
             });

            return mock;
        }
    }
}
