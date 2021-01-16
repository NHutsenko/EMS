using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EMS.Core.API.Models;
using EMS.Core.API.Tests.Mocks;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class PeopleServiceTest : BaseUnitTest
    {
        private Person _person1;
        private Person _person2;
        private Contact _contact;
        private PersonPhoto _photo;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            DbContextMock.ShouldThrowException = false;

            int idPersonOne = 1;
            _contact = new Contact
            {
                PersonId = idPersonOne,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Id = 1,
                ContactType = Enums.ContactType.Phone,
                Name = "test",
                Value = "test"
            };
            _dbContext.Contacts.Add(_contact);

            _photo = new PersonPhoto
            {
                Id = 1,
                Mime = "test",
                Base64 = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "test",
                PersonId = idPersonOne
            };
            _dbContext.Photos.Add(_photo);

            _person1 = new Person
            {
                Id = idPersonOne,
                LastName = "Test",
                Name = "Test",
                SecondName = "Test",
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Contacts = new List<Contact> { _contact },
                Photos = new List<PersonPhoto> { _photo }
            };
            _person2 = new Person
            {
                Id = 2,
                LastName = "Test",
                Name = "Test",
                SecondName = "Test",
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
            };

            _dbContext.People.Add(_person1);
            _dbContext.People.Add(_person2);


            _peopleRepository = new DAL.Repositories.PeopleRepository(_dbContext, _dateTimeUtil);
            _peopleService = new API.Services.PeopleService(_peopleRepository);
        }

        [Test]
        public void GetAll_should_return_people_from_db()
        {
            // Arrange
            PeopleResponse expected = new PeopleResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            _person1.Contacts = null;
            _person1.Photos = null;
            expected.Data.Add(new PersonData 
            { 
                Id = _person1.Id, 
                LastName = _person1.LastName, 
                Name = _person1.Name, 
                SecondName = _person1.SecondName,
                BornedOn = Timestamp.FromDateTime(_person1.BornedOn.ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_person1.CreatedOn.ToUniversalTime()),
                
            });
            expected.Data.Add(new PersonData
            {
                Id = _person2.Id,
                LastName = _person2.LastName,
                Name = _person2.Name,
                SecondName = _person2.SecondName,
                BornedOn = Timestamp.FromDateTime(_person2.BornedOn.ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_person2.CreatedOn.ToUniversalTime()),
            });

            // Act
            PeopleResponse actual = _peopleService.GetAll(new Empty(), null).Result;

            // Assert
            Assert.AreEqual(expected.Response.Code, actual.Response.Code, "Code as expected");
            Assert.AreEqual(expected.Response.ErrorMessage, actual.Response.ErrorMessage, "Error message as expected");
            CollectionAssert.AreEqual(expected.Data, actual.Data, "Data as expected");
        }

        [Test]
        public void GetById_should_return_person_by_specified_id()
        {
            // Arrange
            PersonResponse expected = new PersonResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                },
                Data = new PersonData
                {
                    Id = _person1.Id,
                    LastName = _person1.LastName,
                    Name = _person1.Name,
                    SecondName = _person1.SecondName,
                    BornedOn = Timestamp.FromDateTime(_person1.BornedOn.ToUniversalTime()),
                    CreatedOn = Timestamp.FromDateTime(_person1.CreatedOn.ToUniversalTime()),
                }
            };
            expected.Data.Contacts.Add(new ContactData
            {
                PersonId = _contact.PersonId,
                ContactType = (int)_contact.ContactType,
                Name = _contact.Name,
                Value = _contact.Value
            });
            expected.Data.Photos.Add(new PhotoData
            {
                PersonId = _photo.PersonId,
                Base64 = _photo.Base64,
                Mime = _photo.Mime,
                Name = _photo.Name
            });

            // Act
            PersonResponse actual = _peopleService.GetById(new PersonRequest { Id = _person1.Id }, null).Result;

            // Assert
            Assert.AreEqual(expected.Response.Code, actual.Response.Code, "Code as expected");
            Assert.AreEqual(expected.Response.ErrorMessage, actual.Response.ErrorMessage, "Error message as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Data as expected");
        }

        [Test]
        public void GetById_should_throw_null_reference_exception()
        {
            // Arrange
            PersonResponse expected = new PersonResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = "Some data has not found (type: NullReferenceException)"
                },
                Data = null
            };

            // Act
            PersonResponse actual = _peopleService.GetById(new PersonRequest { Id = 3 }, null).Result;

            // Assert
            Assert.AreEqual(expected.Response.Code, actual.Response.Code, "Code as expected");
            Assert.AreEqual(expected.Response.ErrorMessage, actual.Response.ErrorMessage, "Error message as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Data as expected");
        }

        [Test]
        public void GetById_should_throw_exception()
        {
            // Arrange
            PersonResponse expected = new PersonResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = "Value cannot be null. (Parameter 'value')"
                },
                Data = null
            };

            Person person = new Person
            {
                Id = 3
            };
            _dbContext.People.Add(person);

            // Act
            PersonResponse actual = _peopleService.GetById(new PersonRequest { Id = person.Id }, null).Result;

            // Assert
            Assert.AreEqual(expected.Response.Code, actual.Response.Code, "Code as expected");
            Assert.AreEqual(expected.Response.ErrorMessage, actual.Response.ErrorMessage, "Error message as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Data as expected");
        }
    }
}
