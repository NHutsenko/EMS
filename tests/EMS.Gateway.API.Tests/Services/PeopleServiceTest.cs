using System.Diagnostics.CodeAnalysis;
using EMS.Core.API.Models;
using EMS.Core.API.Tests.Mocks;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;

namespace EMS.Core.API.Tests.Services
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

            _person1 = new Person
            {
                Id = 1,
                LastName = "Test",
                Name = "Test",
                SecondName = "Test",
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),  
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

            _contact = new Contact
            {
                PersonId = _person1.Id,
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
                PersonId = _person1.Id
            };
            _dbContext.Photos.Add(_photo);

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
            expected.People.Add(new PersonData 
            { 
                Id = _person1.Id, 
                LastName = _person1.LastName, 
                Name = _person1.Name, 
                SecondName = _person1.SecondName,
                BornedOn = Timestamp.FromDateTime(_person1.BornedOn.ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_person1.CreatedOn.ToUniversalTime()),
            });
            expected.People.Add(new PersonData
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
            CollectionAssert.AreEqual(expected.People, actual.People, "Data as expected");
        }
    }
}
