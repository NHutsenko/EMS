using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Moq;

namespace EMS.Core.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class TeamsRepositoryMock: BaseMock
    {
        public static Mock<TeamsRepository> SetupMock(IApplicationDbContext dbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<TeamsRepository> mock = new(dbContext, dateTimeUtil);
            TeamsRepository repository = new(dbContext, dateTimeUtil);
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

            mock.Setup(m => m.AddAsync(It.IsAny<Team>())).Returns<Team>((team) =>
            {
                return repository.AddAsync(team);
            });

            mock.Setup(m => m.UpdateAsync(It.IsAny<Team>())).Returns<Team>((team) =>
            {
                return repository.UpdateAsync(team);
            });

            mock.Setup(m => m.DeleteAsync(It.IsAny<Team>())).Returns<Team>((team) =>
            {
                return repository.DeleteAsync(team);
            });

            return mock;
        }
    }
}
