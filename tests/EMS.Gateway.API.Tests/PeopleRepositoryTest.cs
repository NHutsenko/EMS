using System.Diagnostics.CodeAnalysis;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using NUnit.Framework;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class PeopleRepositoryTest: BaseUnitTest
    {
        private PeopleRepository _repository;
        private Person _person1;
        private Contact _contact1;
        private Contact _contact2;
        private PersonPhoto _photo1;
        private PersonPhoto _photo2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            _person1 = new Person
            {
                Id = 1,
                Name = "Jonh",
                LastName = "Jonhson",
                SecondName = "Peter",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
            };
            _dbContext.People.Add(_person1);

            _contact1 = new Contact
            {
                Id = 1,
                Name = "Personal Phone",
                Value = "123123123",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ContactType = Enums.ContactType.Phone,
                PersonId = 1
            };

            _contact2 = new Contact
            {
                Id = 2,
                Name = "Personal Phone",
                Value = "123123124",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddDays(1),
                ContactType = Enums.ContactType.Phone,
                PersonId = 1
            };
            _dbContext.Contacts.Add(_contact1);
            _dbContext.Contacts.Add(_contact2);

            _photo1 = new PersonPhoto
            {
                Id = 1,
                PersonId = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Mime = "image/png",
                Base64 = "ejJlQUVFSFpCcUpjVDNlWW5WcEd0QQ=="
            };

            _photo1 = new PersonPhoto
            {
                Id = 2,
                PersonId = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddDays(1),
                Mime = "image/png",
                Base64 = "ejJlQUVFSFpCcUpjVDNlWW5WcEd0QQ=="
            };

            _dbContext.Photos.Add(_photo1);
            _dbContext.Photos.Add(_photo2);

            _repository = new PeopleRepository(_dbContext, _dateTimeUtil);
        }
    }
}
