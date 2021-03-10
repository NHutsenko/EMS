using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Moq;

namespace EMS.Core.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class PositionsRepositoryMock: BaseMock
    {
        public static Mock<PositionsRepository> SetupMock(IApplicationDbContext dbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<PositionsRepository> mock = new Mock<PositionsRepository>(dbContext, dateTimeUtil);
            PositionsRepository repository = new PositionsRepository(dbContext, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<Position>())).Returns<Position>((position) =>
            {
                return repository.AddAsync(position);
            });

            mock.Setup(m => m.UpdateAsync(It.IsAny<Position>())).Returns<Position>((position) =>
            {
                return repository.UpdateAsync(position);
            });

            mock.Setup(m => m.DeleteAsync(It.IsAny<Position>())).Returns<Position>((position) =>
            {
                return repository.DeleteAsync(position);
            });

            mock.Setup(m => m.GetAll()).Returns(() =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetAll();
            });

            mock.Setup(m => m.Get(It.IsAny<long>())).Returns<long>((id) =>
            {
                ThrowExceptionIfNeeded();
                return repository.Get(id);
            });

            return mock;
        }
    }
}
