using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Moq;

namespace EMS.Core.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class PeopleRepositoryMock: BaseMock
    {
        public static Mock<PeopleRepository> SetupMock(IApplicationDbContext dbContext, IDateTimeUtil dateTimeUtil)
        {
            Mock<PeopleRepository> mock = new(dbContext, dateTimeUtil);
            PeopleRepository repository = new(dbContext, dateTimeUtil);

            mock.Setup(m => m.AddAsync(It.IsAny<Person>())).Returns<Person>((person) =>
            {
                return repository.AddAsync(person);
            });

            mock.Setup(m => m.UpdateAsync(It.IsAny<Person>())).Returns<Person>((person) =>
            {
                return repository.UpdateAsync(person);
            });

            mock.Setup(m => m.AddContactAsync(It.IsAny<Contact>())).Returns<Contact>((contact) =>
            {
                return repository.AddContactAsync(contact);
            });

            mock.Setup(m => m.AddPhotoAsync(It.IsAny<PersonPhoto>())).Returns<PersonPhoto>((photo) =>
            {
                return repository.AddPhotoAsync(photo);
            });

            mock.Setup(m => m.GetAll()).Returns(() =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetAll();
            });

            mock.Setup(m => m.GetById(It.IsAny<long>())).Returns<long>((id) =>
            {
                ThrowExceptionIfNeeded();
                return repository.GetById(id);
            });

            return mock;
        }
    }
}
