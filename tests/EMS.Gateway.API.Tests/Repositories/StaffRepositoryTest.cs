using System;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EMS.Core.API.Tests.Mocks;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class StaffRepositoryTest : BaseUnitTest
    {
        public Staff _staff1;
        public Staff _staff2;
        public Position _position1;
        public Position _position2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            _staff1 = new Staff
            {
                Id = 1,
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                ManagerId = 123,
                PersonId = 1,
                PositionId = 1
            };

            _staff2 = new Staff
            {
                Id = 1,
                CreatedOn = new DateTime(2020, 02, 01, 12, 00, 00),
                ManagerId = 123,
                PersonId = 1,
                PositionId = 2
            };
            _dbContext.Staff.Add(_staff1);
            _dbContext.Staff.Add(_staff2);

            _position1 = new Position
            {
                Id = 1
            };

            _position2 = new Position
            {
                Id = 2
            };
            _dbContext.Positions.Add(_position1);
            _dbContext.Positions.Add(_position2);

            _staffRepository = new StaffRepository(_dbContext);
            DbContextMock.ShouldThrowException = false;
        }

        [Test]
        public void AddAsync_should_add_new_record_to_db()
        {
            // Arrange
            Staff toAdd = new Staff
            {
                CreatedOn = new DateTime(2020, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = _position2.Id
            };

            // Act
            int result = _staffRepository.AddAsync(toAdd).Result;
            Staff added = _dbContext.Staff.FirstOrDefault(e => e.Id == toAdd.Id);

            // Assert
            Assert.AreEqual(toAdd, added, "AddAsync added entity as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Staff toAdd = new Staff
            {
                CreatedOn = new DateTime(2020, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = _position2.Id
            };

            // Assert
            Assert.ThrowsAsync<Exception>(() => _staffRepository.AddAsync(toAdd), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_entity_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _staffRepository.AddAsync(null), "AddAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_positionId_is_equal_to_zero()
        {
            // Arrange
            Staff toAdd = new Staff
            {
                CreatedOn = new DateTime(2019, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = 0
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _staffRepository.AddAsync(toAdd), "AddAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_position_does_not_exists_in_db()
        {
            // Arrange
            Staff toAdd = new Staff
            {
                CreatedOn = new DateTime(2020, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = 3
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _staffRepository.AddAsync(toAdd), "AddAsync succesfully throws exception with wrong position");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_managerId_is_equal_to_zero()
        {
            // Arrange
            Staff toAdd = new Staff
            {
                CreatedOn = new DateTime(2019, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 0,
                PositionId = _position2.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _staffRepository.AddAsync(toAdd), "AddAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_because_date_is_less_than_existing_staff_record_in_db()
        {
            // Arrange
            Staff toAdd = new Staff
            {
                CreatedOn = new DateTime(2019, 02, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = _position2.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _staffRepository.AddAsync(toAdd), "AddAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_update_record_in_db()
        {
            // Arrange
            Staff toUpdate = new Staff
            {
                Id = 1,
                CreatedOn = new DateTime(2020, 02, 01, 12, 00, 00),
                ManagerId = 123,
                PersonId = 1,
                PositionId = _position2.Id
            };

            // Act
            int result = _staffRepository.UpdateAsync(toUpdate).Result;

            // Assert
            Assert.AreEqual(toUpdate, _staff1, "Updated as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Staff toUpdate = new Staff
            {
                CreatedOn = new DateTime(2020, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = _position2.Id,
                Id = _staff1.Id
            };

            // Assert
            Assert.ThrowsAsync<Exception>(() => _staffRepository.UpdateAsync(toUpdate), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_entity_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _staffRepository.UpdateAsync(null), "AddAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_positionId_is_equal_to_zero()
        {
            // Arrange
            Staff toUpdate = new Staff
            {
                Id = 2,
                CreatedOn = new DateTime(2020, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = 0
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _staffRepository.UpdateAsync(toUpdate), "AddAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_position_does_not_exists_in_db()
        {
            // Arrange
            Staff toAdd = new Staff
            {
                CreatedOn = new DateTime(2020, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = 3
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _staffRepository.UpdateAsync(toAdd), "UpdateAsync succesfully throws exception with wrong position");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_managerId_is_equal_to_zero()
        {
            // Arrange
            Staff toUpdate = new Staff
            {
                Id = 2,
                CreatedOn = new DateTime(2020, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 0,
                PositionId = 3
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _staffRepository.UpdateAsync(toUpdate), "AddAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_because_date_is_less_than_existing_staff_record_in_db()
        {
            // Arrange
            Staff toUpdate = new Staff
            {
                Id = 2,
                CreatedOn = new DateTime(2019, 02, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = _position1.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _staffRepository.UpdateAsync(toUpdate), "AddAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_succesfully_delete_record_from_db()
        {
            // Arrange
            Staff toDelete = new Staff
            {
                Id = 3,
                CreatedOn = DateTime.Now.AddDays(1),
                PersonId = 1,
                ManagerId = 123,
                PositionId = 3
            };
            _dbContext.Staff.Add(toDelete);

            // Act
            int result = _staffRepository.DeleteAsync(toDelete).Result;

            // Assert
            CollectionAssert.AreEqual(new List<Staff> { _staff1, _staff2 }, _dbContext.Staff.ToList(), "DeleteAsync deleted entity as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Staff toDelete = new Staff
            {
                CreatedOn = new DateTime(2021, 03, 01, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = 3,
                Id = _staff1.Id
            };

            // Assert
            Assert.ThrowsAsync<Exception>(() => _staffRepository.DeleteAsync(toDelete), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_throw_exception_because_trying_to_delete_history_record()
        {
            // Arrange
            Staff toDelete = new Staff
            {
                Id = 3,
                CreatedOn = new DateTime(2020, 01, 15, 12, 00, 00),
                PersonId = 1,
                ManagerId = 123,
                PositionId = 3
            };
            _dbContext.Staff.Add(toDelete);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _staffRepository.DeleteAsync(toDelete), "DeleteAsync throws exception as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void GetAll_should_return_records_from_db()
        {
            // Act
            IQueryable<Staff> actual = _staffRepository.GetAll();

            // Assert
            CollectionAssert.AreEqual(new List<Staff> { _staff1, _staff2 }, actual, "GetAll returned result as expected");
        }

        [Test]
        public void GetByPersonId_should_return_records_from_db()
        {
            // Act
            IQueryable<Staff> actual = _staffRepository.GetByPersonId(1);

            // Assert
            CollectionAssert.AreEqual(new List<Staff> { _staff1, _staff2 }, actual, "GetByPersonId returned result as expected");
        }

        [Test]
        public void GetByManagerId_should_return_records_from_db()
        {
            // Arrange
            Staff byManager = new Staff
            {
                ManagerId = 2
            };
            _dbContext.Staff.Add(byManager);

            // Act
            IQueryable<Staff> actual = _staffRepository.GetByManagerId(byManager.ManagerId);

            // Assert
            CollectionAssert.AreEqual(new List<Staff> { byManager }, actual, "GetByManagerId returned result as expected");
        }
    }
}
