using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Moq;

namespace EMS.Core.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class RoadMapRepositoryMock: BaseMock
    {
        public static Mock<RoadMapRepository> SetupMock(IApplicationDbContext applicationDbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<RoadMapRepository> mock = new(applicationDbContext, dateTimeUtil);
            RoadMapRepository repository = new(applicationDbContext, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<RoadMap>())).Returns<RoadMap>((roadMap) =>
            {
                return repository.AddAsync(roadMap);
            });

            mock.Setup(m => m.DeleteAsync(It.IsAny<RoadMap>())).Returns<RoadMap>((roadMap) =>
            {
                return repository.DeleteAsync(roadMap);
            });

            mock.Setup(m => m.UpdateAsync(It.IsAny<RoadMap>())).Returns<RoadMap>((roadMap) =>
            {
                return repository.UpdateAsync(roadMap);
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
