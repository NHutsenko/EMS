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
    public class StaffRepositoryMock : BaseMock
    {
        public static Mock<StaffRepository> SetupMock(IApplicationDbContext dbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<StaffRepository> mock = new Mock<StaffRepository>(dbContext, dateTimeUtil);
            StaffRepository repository = new StaffRepository(dbContext, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<Staff>())).Returns<Staff>((staff) =>
            {
                return repository.AddAsync(staff);
            });
            mock.Setup(m => m.UpdateAsync(It.IsAny<Staff>())).Returns<Staff>((staff) =>
            {
                return repository.UpdateAsync(staff);
            });
            mock.Setup(m => m.DeleteAsync(It.IsAny<Staff>())).Returns<Staff>((staff) =>
            {
                return repository.DeleteAsync(staff);
            });
            mock.Setup(m => m.GetAll()).Returns(() =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetAll();
            });
            mock.Setup(m => m.GetByManagerId(It.IsAny<long>())).Returns<long>((managerId) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetByManagerId(managerId);
            });
            mock.Setup(m => m.GetByPersonId(It.IsAny<long>())).Returns<long>((personId) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetByPersonId(personId);
            });

            return mock;
        }
    }
}
