using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Moq;

namespace EMS.Core.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MotivationModificatorRepositoryMock: BaseMock
    {
        public static Mock<MotivationModificatorRepository> SetupMock(IApplicationDbContext dbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<MotivationModificatorRepository> mock = new(dbContext, dateTimeUtil);
            MotivationModificatorRepository repository = new(dbContext, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<MotivationModificator>())).Returns<MotivationModificator>((modificator) =>
            {
                return repository.AddAsync(modificator);
            });
            mock.Setup(m => m.UpdateAsync(It.IsAny<MotivationModificator>())).Returns<MotivationModificator>((modificator) =>
            {
                return repository.UpdateAsync(modificator);
            });
            mock.Setup(m => m.GetByStaffId(It.IsAny<long>())).Returns<long>((staffId) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetByStaffId(staffId);
            });

            return mock;
        }
    }
}
