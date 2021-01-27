using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using EMS.Core.API.Tests.Mock;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace EMS.Core.API.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class OtherPaymentsTest: BaseUnitTest<OtherPaymentsRepository>
    {
        private Person _person;
        private OtherPayment _otherPayment1;
        private OtherPayment _otherPayment2;

        public object IQueriable { get; private set; }

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            DbContextMock.ShouldThrowException = false;

            _person = new Person
            {
                Id = 1
            };
            _dbContext.People.Add(_person);

            _otherPayment1 = new OtherPayment
            {
                Id = 1,
                Value = 123.45,
                Comment = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = _person.Id
            };

            _otherPayment2 = new OtherPayment
            {
                Id = 1,
                Value = 123.45,
                Comment = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddMonths(1),
                PersonId = _person.Id
            };
            _dbContext.OtherPayments.Add(_otherPayment1);
            _dbContext.OtherPayments.Add(_otherPayment2);

            _otherPaymentsRepository = new DAL.Repositories.OtherPaymentsRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void GetByPersonId_should_return_all_other_payments_related_to_person()
        {
            // Act
            IQueryable<OtherPayment> actual = _otherPaymentsRepository.GetByPersonId(_person.Id);

            // Assert
            CollectionAssert.AreEqual(new List<OtherPayment> { _otherPayment1, _otherPayment2 }, actual, "Other payments returned as expected");
        }

        [Test]
        public void GetByPersonIdAndDateRange_should_return_all_other_payments_related_to_person_and_renge_date()
        {
            // Act
            IQueryable<OtherPayment> actual = _otherPaymentsRepository.GetByPersonIdAndDateRange(_person.Id, _dateTimeUtil.GetCurrentDateTime().AddDays(15), _dateTimeUtil.GetCurrentDateTime().AddMonths(2) );

            // Assert
            CollectionAssert.AreEqual(new List<OtherPayment> { _otherPayment2 }, actual, "Other payments returned as expected");
        }

        [Test]
        public void GetByPersonIdAndDateRange_should_throw_exception_because_of_date_range_is_wrong()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _otherPaymentsRepository.GetByPersonIdAndDateRange(_person.Id, _dateTimeUtil.GetCurrentDateTime().AddMonths(1), _dateTimeUtil.GetCurrentDateTime()), "Date range expcetion returned as expected");
        }

        [Test]
        public void AddAsync_should_add_other_payment_to_db()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = 10,
                Comment = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = _person.Id
            };

            // Act
            int result = _otherPaymentsRepository.AddAsync(otherPayment).Result;
            OtherPayment actual = _dbContext.OtherPayments.FirstOrDefault(e => e.Id == otherPayment.Id);

            // Assert
            Assert.AreEqual(otherPayment, actual, "Other payment added succesfully");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throws_argument_null_exception_because_of_other_payment_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _otherPaymentsRepository.AddAsync(null), "Argument null exception because of other payment is null throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_argument_exception_because_of_other_payment_value_is_wrong()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = -10,
                Comment = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = _person.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _otherPaymentsRepository.AddAsync(otherPayment), "Argument exception because value less or equal to 0 throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_argument_exception_because_of_other_payment_comment_is_empty()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = 10,
                Comment = string.Empty,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = _person.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _otherPaymentsRepository.AddAsync(otherPayment), "Argument exception because comment is empty as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_argument_exception_because_of_other_payment_creation_date_is_wrong()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = 10,
                Comment = "Test",
                CreatedOn = DateTime.MinValue,
                PersonId = _person.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _otherPaymentsRepository.AddAsync(otherPayment), "Argument exception because creation date is wrong as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_argument_exception_because_of_other_payment_personId_is_wrong()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = 10,
                Comment = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _otherPaymentsRepository.AddAsync(otherPayment), "Argument exception because person Id is wrong as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_update_other_payment_into_db()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = 10,
                Comment = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = _person.Id,
                Id = 1
            };

            // Act
            int result = _otherPaymentsRepository.UpdateAsync(otherPayment).Result;
            OtherPayment actual = _dbContext.OtherPayments.FirstOrDefault(e => e.Id == otherPayment.Id);

            // Assert
            Assert.AreEqual(otherPayment, actual, "Other payment updated succesfully");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throws_argument_null_exception_because_of_other_payment_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _otherPaymentsRepository.UpdateAsync(null), "Argument null exception because of other payment is null throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_argument_exception_because_of_other_payment_value_is_wrong()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = -10,
                Comment = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = _person.Id,
                Id = 1
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _otherPaymentsRepository.UpdateAsync(otherPayment), "Argument exception because value less or equal to 0 throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_argument_exception_because_of_other_payment_comment_is_empty()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = 10,
                Comment = string.Empty,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = _person.Id,
                Id = 1
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _otherPaymentsRepository.UpdateAsync(otherPayment), "Argument exception because comment is empty as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_argument_exception_because_of_other_payment_creation_date_is_wrong()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = 10,
                Comment = "Test",
                CreatedOn = DateTime.MinValue,
                PersonId = _person.Id,
                Id = 1
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _otherPaymentsRepository.UpdateAsync(otherPayment), "Argument exception because creation date is wrong as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_argument_exception_because_of_other_payment_personId_is_wrong()
        {
            // Arrange
            OtherPayment otherPayment = new OtherPayment
            {
                Value = 10,
                Comment = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Id = 1
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _otherPaymentsRepository.UpdateAsync(otherPayment), "Argument exception because person Id is wrong as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_succesfully_delete_record_from_db()
        {
            // Act
            int result = _otherPaymentsRepository.DeleteAsync(_otherPayment2).Result;

            // Assert
            CollectionAssert.AreEqual(new List<OtherPayment> { _otherPayment1 }, _dbContext.OtherPayments.ToList(), "Record deleted as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_throw_exception_because_of_record_is_not_from_current_month_or_later()
        {
            // Arrange
            _otherPayment2.CreatedOn = _otherPayment2.CreatedOn.AddMonths(-3);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _otherPaymentsRepository.DeleteAsync(_otherPayment2), "exception throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }
    }
}
