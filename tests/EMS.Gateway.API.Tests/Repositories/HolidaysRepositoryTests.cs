using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using EMS.Core.API.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class HolidaysRepositoryTests: BaseUnitTest
    {
        private Holiday _holiday1;
        private Holiday _holiday2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            DbContextMock.ShouldThrowException = false;

            _holiday1 = new Holiday
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Description = "Test 1",
                HolidayDate = new DateTime(2020, 1, 7)
            };

            _holiday2 = new Holiday
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Description = "Test 2",
                HolidayDate = new DateTime(2020, 4, 20)
            };
            _dbContext.Holidays.Add(_holiday1);
            _dbContext.Holidays.Add(_holiday2);

            _holidaysRepository = new HolidaysRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void GetAll_should_return_all_holidays_from_db()
        {
            // Assert
            CollectionAssert.AreEqual(new List<Holiday> { _holiday1, _holiday2 }, _holidaysRepository.GetAll(), "Returned holidays as expected");
        }

        [Test]
        public void GetByDateRange_should_return_all_holidays_from_db_by_date_range()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 4, 1);
            DateTime endDate = new DateTime(2020, 4, 25);

            // Assert
            CollectionAssert.AreEqual(new List<Holiday> {_holiday2 }, _holidaysRepository.GetByDateRange(startDate, endDate), "Returned holidays as expected");
        }

        [Test]
        public void GetByDateRange_should_throw_exception_because_of_date_range_is_wrong()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 4, 25);
            DateTime endDate = new DateTime(2020, 4, 1);

            // Assert
            Assert.Throws<ArgumentException>(() => _holidaysRepository.GetByDateRange(startDate, endDate), "Throws wrong date range exception as expected");
        }

        [Test]
        public void AddAsync_should_add_holiday_to_db()
        {
            // Arrange
            Holiday holiday = new Holiday
            {
                Description = "New holiday",
                HolidayDate = new DateTime(2020, 08, 01),
                ToDoDate = new DateTime(2020, 08, 05)
            };

            // Act
            int result = _holidaysRepository.AddAsync(holiday).Result;
            Holiday actual = _dbContext.Holidays.FirstOrDefault(e => e.Id == holiday.Id);

            // Assert
            Assert.AreEqual(holiday, actual, "Entity added as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throws_exception_because_of_holiday_entity_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _holidaysRepository.AddAsync(null), "Throws null reference exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_exception_because_of_holiday_description_is_null()
        {
            // Arrange
            Holiday holiday = new Holiday
            {
                Description = string.Empty,
                HolidayDate = new DateTime(2020, 08, 01),
                ToDoDate = new DateTime(2020, 08, 05)
            };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _holidaysRepository.AddAsync(holiday), "Throws argument null exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_exception_because_of_holiday_date_is_wrong()
        {
            // Arrange
            Holiday holiday = new Holiday
            {
                Description = "Test",
                HolidayDate = DateTime.MinValue,
                ToDoDate = new DateTime(2020, 08, 05)
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _holidaysRepository.AddAsync(holiday), "Throws argument exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_update_holiday_into_db()
        {
            // Arrange
            Holiday holiday = new Holiday
            {
                Description = "New holiday",
                HolidayDate = new DateTime(2020, 08, 01),
                ToDoDate = new DateTime(2020, 08, 05),
                Id = _holiday1.Id
            };

            // Act
            int result = _holidaysRepository.UpdateAsync(holiday).Result;
            Holiday actual = _dbContext.Holidays.FirstOrDefault(e => e.Id == holiday.Id);

            // Assert
            Assert.AreEqual(holiday, actual, "Entity updated as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throws_exception_because_of_holiday_entity_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _holidaysRepository.UpdateAsync(null), "Throws null reference exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_exception_because_of_holiday_description_is_null()
        {
            // Arrange
            Holiday holiday = new Holiday
            {
                Description = string.Empty,
                HolidayDate = new DateTime(2020, 08, 01),
                ToDoDate = new DateTime(2020, 08, 05),
                Id = _holiday1.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _holidaysRepository.UpdateAsync(holiday), "Throws argument null exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_exception_because_of_holiday_date_is_wrong()
        {
            // Arrange
            Holiday holiday = new Holiday
            {
                Description = "Test",
                HolidayDate = DateTime.MinValue,
                ToDoDate = new DateTime(2020, 08, 05)
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _holidaysRepository.UpdateAsync(holiday), "Throws argument exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_delete_record_from_db()
        {
            // Act
            int result = _holidaysRepository.DeleteAsync(_holiday2).Result;

            // Assert
            CollectionAssert.AreEqual(new List<Holiday> { _holiday1 }, _dbContext.Holidays.ToList(), "Deleted record as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_throw_exception_because_record_from_history()
        {
            // Arrange
            Holiday holiday = new Holiday
            {
                Description = "Test",
                HolidayDate = new DateTime(2019, 08, 05),
                Id = 3
            };
            _dbContext.Holidays.Add(holiday);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _holidaysRepository.DeleteAsync(holiday), "Throws argument exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }
    }
}
