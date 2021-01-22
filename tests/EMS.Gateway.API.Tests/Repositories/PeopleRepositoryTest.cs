using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Enums;
using EMS.Core.API.Models;
using EMS.Core.API.Tests.Mock;
using Microsoft.EntityFrameworkCore;
using Moq;
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
            DbContextMock.ShouldThrowException = false;

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

        [Test]
        public void AddAsync_should_add_person_data_to_db()
        {
            // Arrange
            Person person = new Person
            {
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "Test",
                LastName = "Test",
                SecondName = "Test"
            };

            // Act
            int result = _peopleRepository.AddAsync(person).Result;
            Person actual = _dbContext.People.FirstOrDefault(e => e.Id == person.Id);

            // Assert
            Assert.AreEqual(person, actual, "Person added to db as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Person person = new Person
            {
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "Test",
                LastName = "Test",
                SecondName = "Test"
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _peopleRepository.AddAsync(person), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_person_data_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _peopleRepository.AddAsync(null), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_last_name_is_invalid()
        {
            // Arrange
            Person person = new Person
            {
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "Test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddAsync(person), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_name_is_invalid()
        {
            // Arrange
            Person person = new Person
            {
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                LastName = "Test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddAsync(person), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_born_date_is_invalid()
        {
            // Arrange
            Person person = new Person
            {
                BornedOn = DateTime.MinValue,
                LastName = "Test",
                Name = "Test",
                SecondName = "Test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddAsync(person), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void Update_should_update_person_data_to_db()
        {
            // Arrange
            Person person = new Person
            {
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "Test",
                LastName = "Test",
                SecondName = "Test",
                Id = 1
            };

            // Act
            int result = _peopleRepository.UpdateAsync(person).Result;

            // Assert
            Assert.AreEqual(person, _person1, "Person added to db as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Person person = new Person
            {
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "Test",
                LastName = "Test",
                SecondName = "Test",
                Id = 1
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _peopleRepository.UpdateAsync(person), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_person_data_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _peopleRepository.UpdateAsync(null), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_last_name_is_invalid()
        {
            // Arrange
            Person person = new Person
            {
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "Test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.UpdateAsync(person), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_name_is_invalid()
        {
            // Arrange
            Person person = new Person
            {
                BornedOn = _dateTimeUtil.GetCurrentDateTime(),
                LastName = "Test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.UpdateAsync(person), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_born_date_is_invalid()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Person person = new Person
            {
                BornedOn = DateTime.MinValue,
                LastName = "Test",
                Name = "Test",
                SecondName = "Test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.UpdateAsync(person), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddContactAsync_should_add_contact_to_db()
        {
            // Arrange
            Contact contact = new Contact
            {
                ContactType = ContactType.Messenger,
                Name = "Telegram",
                PersonId = _person1.Id,
                Value = "test"
            };

            // Act
            int result = _peopleRepository.AddContactAsync(contact).Result;
            Contact expected = _dbContext.Contacts.FirstOrDefault(e => e.Id == contact.Id);

            // Assert
            Assert.AreEqual(contact, expected, "Contact added to db as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }


        [Test]
        public void AddContactAsync_should_throw_exception_because_contact_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _peopleRepository.AddContactAsync(null), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddContactAsync_should_throws_expection_because_contact_value_is_empty()
        {
            // Arrange
            Contact contact = new Contact
            {
                ContactType = ContactType.Messenger,
                Name = "Telegram",
                PersonId = _person1.Id,
                Value = string.Empty
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddContactAsync(contact), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddContactAsync_should_throws_expection_because_person_id_bot_found_in_db()
        {
            // Arrange
            Contact contact = new Contact
            {
                ContactType = ContactType.Messenger,
                Name = "Telegram",
                PersonId = 2,
                Value = "Test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddContactAsync(contact), "Exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddPhotoAsync_should_add_photo_entity_to_db()
        {
            // Arrange
            PersonPhoto personPhoto = new PersonPhoto
            {
                PersonId = _person1.Id,
                Name = "1.jpg",
                Base64 = "dGVzdCBmaWxlIG9uZQ=="
            };

            // Act
            int result = _peopleRepository.AddPhotoAsync(personPhoto).Result;
            PersonPhoto actual = _dbContext.Photos.FirstOrDefault(e => e.Id == personPhoto.Id);

            // Assert
            Assert.AreEqual(personPhoto, actual, "Person photo added to db as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddPhotoAsync_should_throws_exception_because_photo_data_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _peopleRepository.AddPhotoAsync(null), "exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddPhotoAsync_should_throws_exception_because_photo_name_is_empty()
        {
            // Arrange
            PersonPhoto personPhoto = new PersonPhoto
            {
                PersonId = _person1.Id,
                Name = string.Empty,
                Base64 = "dGVzdCBmaWxlIG9uZQ=="
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddPhotoAsync(personPhoto), "exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddPhotoAsync_should_throws_exception_because_person_id_not_found_in_db()
        {
            // Arrange
            PersonPhoto personPhoto = new PersonPhoto
            {
                PersonId = 2,
                Name = "Test.jpg",
                Base64 = "dGVzdCBmaWxlIG9uZQ=="
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddPhotoAsync(personPhoto), "exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddPhotoAsync_should_throws_exception_because_photo_extension_mime_type_is_wrong()
        {
            // Arrange
            PersonPhoto personPhoto = new PersonPhoto
            {
                PersonId = _person1.Id,
                Name = "1.tst",
                Base64 = "dGVzdCBmaWxlIG9uZQ=="
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddPhotoAsync(personPhoto), "exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddPhotoAsync_should_throws_exception_because_base64_is_invalid()
        {
            // Arrange
            PersonPhoto personPhoto = new PersonPhoto
            {
                PersonId = _person1.Id,
                Name = "1.txt",
                Base64 = "test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _peopleRepository.AddPhotoAsync(personPhoto), "exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }
    }
}
