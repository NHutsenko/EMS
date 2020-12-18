using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using EMS.Core.API.Tests.Mocks;
using NUnit.Framework;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class PeopleRepositoryTest : BaseUnitTest
    {
        private Person _person1;
        private Contact _contact1;
        private Contact _contact2;
        private Contact _contact3;
        private PersonPhoto _photo1;
        private PersonPhoto _photo2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();

            _contact1 = new Contact
            {
                Id = 1,
                Name = "Personal Phone",
                Value = "123123123",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ContactType = Enums.ContactType.Phone,
                PersonId = 1,
                Person = _person1
            };

            _contact2 = new Contact
            {
                Id = 2,
                Name = "Personal Phone",
                Value = "123123124",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddDays(1),
                ContactType = Enums.ContactType.Phone,
                PersonId = 1,
                Person = _person1
            };

            _contact3 = new Contact
            {
                Id = 2,
                Name = "Work Email",
                Value = "test@test.test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ContactType = Enums.ContactType.Email,
                PersonId = 1,
                Person = _person1
            };

            _dbContext.Contacts.Add(_contact1);
            _dbContext.Contacts.Add(_contact2);
            _dbContext.Contacts.Add(_contact3);

            _photo1 = new PersonPhoto
            {
                Id = 1,
                PersonId = 1,
                Person = _person1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Mime = "image/png",
                Base64 = "ejJlQUVFSFpCcUpjVDNlWW5WcEd0QQ=="
            };

            _photo2 = new PersonPhoto
            {
                Id = 2,
                PersonId = 1,
                Person = _person1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddDays(1),
                Mime = "image/png",
                Base64 = "ejJlQUVFSFpCcUpjVDNlWW5WcEd0QQ=="
            };

            _dbContext.Photos.Add(_photo1);
            _dbContext.Photos.Add(_photo2);

            _person1 = new Person
            {
                Id = 1,
                Name = "Jonh",
                LastName = "Jonhson",
                SecondName = "Peter",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                Contacts = new List<Contact> { _contact1, _contact2, _contact3 },
                Photos = new List<PersonPhoto> { _photo1, _photo2 }
            };
            _dbContext.People.Add(_person1);

            _peopleRepository = new PeopleRepository(_dbContext, _dateTimeUtil);
            DbContextMock.ShouldThrowException = false;
        }

        [Test]
        public void GetAll_should_return_people_data_from_db()
        {
            // Act
            IQueryable<Person> people = _peopleRepository.GetAll();

            // Assert
            CollectionAssert.AreEqual(new List<Person> { _person1 }, people, "Data recieved as expected");
        }

        [Test]
        public void GetById_should_return_person_info_with_actual_photo_and_contacts()
        {
            // Arrange
            Person expected = new Person
            {
                Id = _person1.Id,
                CreatedOn = _person1.CreatedOn,
                BornedOn = _person1.BornedOn,
                Name = _person1.Name,
                LastName = _person1.LastName,
                SecondName = _person1.SecondName,
                Contacts = new List<Contact> { _contact2, _contact3 },
                Photos = new List<PersonPhoto> { _photo2 }
            };


            // Act
            Person actual = _peopleRepository.GetById(_person1.Id);

            // Assert
            Assert.AreEqual(expected, actual, "Person data returned as expected");
        }
    }
}
