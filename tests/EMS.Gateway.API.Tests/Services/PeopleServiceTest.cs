using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Core.API.Models;
using EMS.Core.API.Services;
using EMS.Core.API.Tests.Mock;
using Google.Protobuf.WellKnownTypes;
using Moq;
using NUnit.Framework;

namespace EMS.Core.API.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class PeopleServiceTest : BaseUnitTest<PeopleService>
    {
        private Person _person1;
        private Person _person2;
        private Contact _contact;
        private PersonPhoto _photo;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new PeopleService(null, null, null));

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
            
            _peopleService = new PeopleService(_peopleRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetAll_should_return_people_from_db()
        {
            // Arrange
            PeopleResponse expected = new PeopleResponse
            {
                Status = new BaseResponse
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

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.GetAll),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = new Empty(),
                Response = expected
            };

            // Act
            PeopleResponse actual = _peopleService.GetAll(new Empty(), null).Result;

            // Assert
            Assert.AreEqual(expected.Status.Code, actual.Status.Code, "Code as expected");
            Assert.AreEqual(expected.Status.ErrorMessage, actual.Status.ErrorMessage, "Error message as expected");
            CollectionAssert.AreEqual(expected.Data, actual.Data, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_return_person_by_specified_id()
        {
            // Arrange
            PersonResponse expected = new PersonResponse
            {
                Status = new BaseResponse
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

            PersonRequest request = new PersonRequest { Id = _person1.Id };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.GetById),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expected
            };

            // Act
            PersonResponse actual = _peopleService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expected.Status.Code, actual.Status.Code, "Code as expected");
            Assert.AreEqual(expected.Status.ErrorMessage, actual.Status.ErrorMessage, "Error message as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_throw_null_reference_exception()
        {
            // Arrange
            PersonResponse expected = new PersonResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = "Some data has not found (type: NullReferenceException)"
                },
                Data = null
            };

            PersonRequest request = new PersonRequest { Id = 3 };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.GetById),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Object reference not set to an instance of an object.")
            };

            // Act
            PersonResponse actual = _peopleService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expected.Status.Code, actual.Status.Code, "Code as expected");
            Assert.AreEqual(expected.Status.ErrorMessage, actual.Status.ErrorMessage, "Error message as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_throw_exception()
        {
            // Arrange
            PersonResponse expected = new PersonResponse
            {
                Status = new BaseResponse
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

            PersonRequest request = new PersonRequest { Id = person.Id };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.GetById),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Value cannot be null. (Parameter 'value')")
            };

            // Act
            PersonResponse actual = _peopleService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expected.Status.Code, actual.Status.Code, "Code as expected");
            Assert.AreEqual(expected.Status.ErrorMessage, actual.Status.ErrorMessage, "Error message as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_Add_person_to_db()
        {
            // Arrange
            PersonData person = new PersonData
            {
                Name = "Test",
                LastName = "Test",
                SecondName = "Test",
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = 3
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = person,
                Response = expected
            };

            // Act
            BaseResponse actual = _peopleService.AddAsync(person, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_null_reference_exception_from_people_repository()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Person data cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Null reference exception handled as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception_from_people_repository()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Person first name or last name cannot be empty"
            };
            PersonData person = new PersonData
            {
                Name = "Test",
                LastName = string.Empty,
                SecondName = "Test",
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = person,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddAsync(person, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Argument exception handled as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_db_update_exception_from_people_repository()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving person data"
            };
            PersonData person = new PersonData
            {
                Name = "Test",
                LastName = "Test",
                SecondName = "Test",
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            DbContextMock.ShouldThrowException = true;

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = person,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _peopleService.AddAsync(person, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Db update exception handled as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_return_unknown_error_based_on_general_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            PersonData person = new PersonData
            {
                Name = "Test",
                LastName = "Test",
                SecondName = "Test",
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                Id = 4
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Person data has not been saved"
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = person,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddAsync(person, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_update_person_int_db()
        {
            // Arrange
            PersonData person = new PersonData
            {
                Name = "Test",
                LastName = "Test",
                SecondName = "Test",
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                Id = _person1.Id
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = person.Id
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = person,
                Response = expected
            };

            // Act
            BaseResponse actual = _peopleService.UpdateAsync(person, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_return_unknown_error_based_on_general_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            PersonData person = new PersonData
            {
                Name = "Test",
                LastName = "Test",
                SecondName = "Test",
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                Id = 4
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Person data has not been updated"
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = person,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.UpdateAsync(person, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_null_reference_exception_from_people_repository()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Person data cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.UpdateAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Null reference exception handled as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception_from_people_repository()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Person first name or last name cannot be empty"
            };
            PersonData person = new PersonData
            {
                Name = "Test",
                LastName = string.Empty,
                SecondName = "Test",
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                Id = _person1.Id
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = person,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.UpdateAsync(person, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Argument exception handled as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_db_update_exception_from_people_repository()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving person data"
            };
            PersonData person = new PersonData
            {
                Name = "Test",
                LastName = "Test",
                SecondName = "Test",
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
                Id = _person1.Id
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = person,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _peopleService.UpdateAsync(person, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Db update exception handled as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddContactAsync_should_add_contact_to_db_via_people_repository()
        {
            // Arrange
            ContactData contact = new ContactData
            {
                PersonId = _person1.Id,
                ContactType = 1,
                Name = "Test",
                Value = "Test"
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = 2
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddContactAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = contact,
                Response = expected
            };

            // Act
            BaseResponse actual = _peopleService.AddContactAsync(contact, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Saved via people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddContactAsync_should_handle_null_reference_exception_from_people_repository()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Contact data cannot be empty"
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddContactAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddContactAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled NullReferenceException from people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddContactAsync_should_handle_argument_exception_from_people_repository()
        {
            // Arrange
            ContactData contact = new ContactData
            {
                PersonId = 3,
                ContactType = 1,
                Name = "Test",
                Value = "Test"
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Person is not specified"
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddContactAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = contact,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddContactAsync(contact, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled ArgumentException from people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddContactAsync_should_handle_db_update_exception_from_people_repository()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            ContactData contact = new ContactData
            {
                PersonId = _person1.Id,
                ContactType = 1,
                Name = "Test",
                Value = "Test"
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving contact"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddContactAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = contact,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _peopleService.AddContactAsync(contact, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled DbUpdateException from people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddContactAsync_should_handle_exception_from_people_repository()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            ContactData contact = new ContactData
            {
                PersonId = _person1.Id,
                ContactType = 1,
                Name = "Test",
                Value = "Test"
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Contact has not been saved"
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddContactAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = contact,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddContactAsync(contact, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled exception from people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddPhotoAsync_should_add_contact_to_db_via_people_repository()
        {
            // Arrange
            PhotoData photoData = new PhotoData
            {
                PersonId = _person1.Id,
                Name = "test.jpg",
                Base64 = "dGVzdCBmaWxlIG9uZQ==",
                Mime = string.Empty
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = 2
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddPhotoAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = photoData,
                Response = expected
            };

            // Act
            BaseResponse actual = _peopleService.AddPhotoAsync(photoData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Saved via people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddPhotoAsync_should_handle_null_reference_exception_from_people_repository()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Photo data cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddPhotoAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddPhotoAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled NullReferenceException from people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddPhotoAsync_should_handle_argument_exception_from_people_repository()
        {
            // Arrange
            PhotoData photoData = new PhotoData
            {
                PersonId = 3,
                Name = "test.jpg",
                Base64 = "dGVzdCBmaWxlIG9uZQ==",
                Mime = string.Empty
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Person is not specified"
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddPhotoAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = photoData,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddPhotoAsync(photoData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled ArgumentException from people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddPhotoAsync_should_handle_db_update_exception_from_people_repository()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            PhotoData photoData = new PhotoData
            {
                PersonId = _person1.Id,
                Name = "test.jpg",
                Base64 = "dGVzdCBmaWxlIG9uZQ==",
                Mime = string.Empty
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving contact"
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddPhotoAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = photoData,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _peopleService.AddPhotoAsync(photoData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled DbUpdateException from people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddPhotoAsync_should_handle_exception_from_people_repository()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            PhotoData photoData = new PhotoData
            {
                PersonId = _person1.Id,
                Name = "test.jpg",
                Base64 = "dGVzdCBmaWxlIG9uZQ==",
                Mime = string.Empty
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Photo has not been saved"
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(PeopleService),
                CallerMethodName = nameof(_peopleService.AddPhotoAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = photoData,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _peopleService.AddPhotoAsync(photoData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled exception from people repository as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
