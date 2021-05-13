using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using EMS.Core.API.Models;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using EMS.Core.API.Tests.Mock;

namespace EMS.Core.API.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class DaysOffRepositoryTests : BaseUnitTest<DayOffRepository>
    {
        private Person _person;
        private DayOff _dayOff1;
        private DayOff _dayOff2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            DbContextMock.ShouldThrowException = false;
            _person = new Person
            {
                Id = 1,
            };
            _dbContext.People.Add(_person);

            _dayOff1 = new DayOff
            {
                Id = 1,
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                PersonId = _person.Id,
                Hours = 8,
                DayOffType = DayOffType.Vacation
            };

            _dayOff2 = new DayOff
            {
                Id = 2,
                CreatedOn = new DateTime(2020, 01, 02, 12, 00, 00),
                PersonId = _person.Id,
                Hours = 8,
                DayOffType = DayOffType.Vacation
            };

            _dbContext.DaysOff.Add(_dayOff1);
            _dbContext.DaysOff.Add(_dayOff2);

            _dayOffRepository = new DayOffRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void AddAsync_should_add_new_record_to_db()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Hours = 8,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Act
            int result = _dayOffRepository.AddAsync(dayOff).Result;
            DayOff expected = _dbContext.DaysOff.FirstOrDefault(e => e.Id == dayOff.Id);

            // Assert
            Assert.AreEqual(expected, dayOff, "Added data as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            DayOff dayOff = new()
            {
                Hours = 8,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _dayOffRepository.AddAsync(dayOff), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throws_exception_because_dayoff_entity_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _dayOffRepository.AddAsync(null), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_exception_because_person_Id_is_not_specified()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Hours = 8,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dayOffRepository.AddAsync(dayOff), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_exception_because_hours_is_less_or_equal_to_zero()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Hours = -1,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dayOffRepository.AddAsync(dayOff), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_exception_because_hours_is_greater_than_max_work_hours()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Hours = 9,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dayOffRepository.AddAsync(dayOff), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_exception_because_date_is_not_specified()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Hours = 8,
                CreatedOn = DateTime.MinValue,
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dayOffRepository.AddAsync(dayOff), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_not_ad_new_record_because_record_with_this_date_already_exists()
        {
            // Arrange
            DayOff dayOff = new()
			{
				Hours = 8,
				CreatedOn = new DateTime(2020, 01, 02, 12, 00, 00),
				DayOffType = DayOffType.SickLeave,
				PersonId = _person.Id
			};

            // Act
            int result = _dayOffRepository.AddAsync(dayOff).Result;

            // Assert
            Assert.AreEqual(0, result, "Result as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_succesfullty_update_record()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Id = 1,
                Hours = 6,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Act
            int result = _dayOffRepository.UpdateAsync(dayOff).Result;

            // Assert
            Assert.AreEqual(dayOff, _dayOff1, "Updated succesfully");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            DayOff dayOff = new()
            {
                Hours = 8,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _dayOffRepository.UpdateAsync(dayOff), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throws_exception_because_dayoff_entity_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _dayOffRepository.UpdateAsync(null), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_exception_because_person_Id_is_not_specified()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Hours = 8,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dayOffRepository.UpdateAsync(dayOff), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_exception_because_hours_is_less_or_equal_to_zero()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Id = 1,
                Hours = -1,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dayOffRepository.UpdateAsync(dayOff), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_exception_because_hours_is_greater_than_max_work_hours()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Id = 1,
                Hours = 9,
                CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dayOffRepository.UpdateAsync(dayOff), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_exception_because_date_is_not_specified()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Id = 1,
                Hours = 8,
                CreatedOn = DateTime.MinValue,
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dayOffRepository.UpdateAsync(dayOff), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_delete_record_from_db()
        {
            // Arrange
            DayOff dayOff = new()
            {
                Id = 3,
                Hours = 8,
                CreatedOn = new DateTime(2021, 4, 20),
                DayOffType = DayOffType.SickLeave,
                PersonId = _person.Id,
            };
            _dbContext.DaysOff.Add(dayOff);

            // Act
            int result = _dayOffRepository.DeleteAsync(dayOff).Result;

            // Assert
            CollectionAssert.AreEqual(new List<DayOff> { _dayOff1, _dayOff2 }, _dbContext.DaysOff.ToList(), "DeleteAsync deleted as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            DayOff dayOff = new()
			{
				Hours = 8,
				CreatedOn = new DateTime(2021, 01, 15, 12, 00, 00),
				DayOffType = DayOffType.SickLeave,
				PersonId = _person.Id
			};

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _dayOffRepository.DeleteAsync(dayOff), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_throws_exception_because_dayoff_entity_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _dayOffRepository.DeleteAsync(null), "Throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_throws_invalid_operation_exception()
        {
            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _dayOffRepository.DeleteAsync(_dayOff1), "DeleteAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void GetByPersobId_should_return_all_records_from_db_by_specified_personId()
        {
            // Act
            IQueryable<DayOff> result = _dayOffRepository.GetByPersonId(_person.Id);

            // Assert
            CollectionAssert.AreEqual(new List<DayOff> { _dayOff1, _dayOff2 }, result, "Result as expected");
        }

        [Test]
        public void GetByStaffId_should_throw_exception_because_staffId_is_zero()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _dayOffRepository.GetByPersonId(0), "Throws exception as expected");
        }

        [Test]
        public void GetByDateRange_should_return_all_records_from_db_by_specified_DateRange()
        {
            // Arrange
            DayOff dayOff = new()
			{
				Id = 3,
				Hours = 8,
				CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
				DayOffType = DayOffType.SickLeave,
				PersonId = _person.Id
			};
            _dbContext.DaysOff.Add(dayOff);

            // Act
            IQueryable<DayOff> result = _dayOffRepository.GetByDateRange(new DateTime(2020, 01, 01, 12, 00, 00), new DateTime(2020, 01, 02, 12, 00, 00));

            // Assert
            CollectionAssert.AreEqual(new List<DayOff> { _dayOff1, _dayOff2 }, result, "Result as expected");
        }

        [Test]
        public void GetByDateRange_should_throw_exception_because_date_range_is_wrong()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _dayOffRepository.GetByDateRange(new DateTime(2020, 01, 02, 12, 00, 00), new DateTime(2020, 01, 01, 12, 00, 00)), "Throws exception as expected");
        }

        [Test]
        public void GetByDateRangeAndStaffId_should_return_all_records_from_db_by_specified_DateRange_and_staffId()
        {
            // Arrange
            DayOff dayOff = new()
			{
				Id = 3,
				Hours = 8,
				CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
				DayOffType = DayOffType.SickLeave,
				PersonId = _person.Id
			};
            DayOff staffNotForSearch = new()
			{
				Id = 3,
				Hours = 8,
				CreatedOn = new DateTime(2020, 01, 03, 12, 00, 00),
				DayOffType = DayOffType.SickLeave,
				PersonId = 2
			};
            _dbContext.DaysOff.Add(dayOff);
            _dbContext.DaysOff.Add(staffNotForSearch);

            // Act
            IQueryable<DayOff> result = _dayOffRepository.GetByDateRangeAndPersonId(new DateTime(2020, 01, 02, 12, 00, 00), new DateTime(2020, 01, 03, 12, 00, 00), _person.Id);

            // Assert
            CollectionAssert.AreEqual(new List<DayOff> { _dayOff2, dayOff }, result, "Result as expected");
        }

        [Test]
        public void GetByDateRangeAndStaffId_should_throw_exception_because_staffId_is_zero()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _dayOffRepository.GetByDateRangeAndPersonId(new DateTime(2020, 01, 01, 12, 00, 00), new DateTime(2020, 01, 02, 12, 00, 00), 0), "Throws exception as expected");
        }
    }
}
