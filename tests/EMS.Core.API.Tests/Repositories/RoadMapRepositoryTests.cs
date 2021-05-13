using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Moq;
using NUnit.Framework;

namespace EMS.Core.API.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RoadMapRepositoryTests : BaseUnitTest<RoadMapRepository>
    {
        private RoadMap _roadMap;
        private Staff _staff1;
        private Staff _staff2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            _staff1 = new Staff
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ManagerId = 1,
                PersonId = 2,
                PositionId = 1
            };

            _staff2 = new Staff
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddMonths(3),
                ManagerId = 1,
                PersonId = 2,
                PositionId = 2
            };

            _roadMap = new RoadMap
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Status = Enums.RoadMapStatus.InProgress,
                Tasks = "test",
                StaffId = _staff1.Id,
                Staff = _staff1
            };
            _staff1.RoadMap = _roadMap;
            _staff1.RoadMapId = _roadMap.Id;

            _dbContext.Staff.Add(_staff1);
            _dbContext.Staff.Add(_staff2);
            _dbContext.RoadMaps.Add(_roadMap);

            _roadMapRepository = new RoadMapRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void GetByStaffId_should_return_road_map_by_staff_id()
        {
            // Act
            RoadMap actual = _roadMapRepository.GetByStaffId(_staff1.Id);

            // Assert
            Assert.AreEqual(_roadMap, actual, "Returned road map by staff id as expected");
        }

        [Test]
        public void AddAsync_should_add_road_map_to_db()
        {
            // Arrange
            RoadMap roadMap = new()
            {
                Status = Enums.RoadMapStatus.Created,
                StaffId = _staff2.Id,
                Tasks = "test"
            };

            // Act
            _ = _roadMapRepository.AddAsync(roadMap).Result;

            // Assert
            CollectionAssert.Contains(_dbContext.RoadMaps.ToList(), roadMap, "RoadMap saved as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_exception_that_road_map_already_exists_for_selected_staff()
        {
            // Arrange
            RoadMap roadMap = new()
            {
                Status = Enums.RoadMapStatus.Created,
                StaffId = _staff1.Id,
                Tasks = "test"
            };

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _roadMapRepository.AddAsync(roadMap), "Throws exception that road map already exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_that_staff_does_not_exists()
        {
            // Arrange
            RoadMap roadMap = new()
            {
                Status = Enums.RoadMapStatus.Created,
                StaffId = 3,
                Tasks = "test"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _roadMapRepository.AddAsync(roadMap), "Throws exception that staff does not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_that_tasks_was_not_provided()
        {
            // Arrange
            RoadMap roadMap = new()
            {
                Status = Enums.RoadMapStatus.Created,
                StaffId = _staff2.Id,
                Tasks = string.Empty
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _roadMapRepository.AddAsync(roadMap), "Throws exception that tasks was not provided");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_update_road_map_data_into_db()
        {
            // Arrange
            RoadMap roadMap = new()
            {
                Id = _roadMap.Id,
                CreatedOn = _roadMap.CreatedOn,
                StaffId = _roadMap.StaffId,
                Tasks = _roadMap.Tasks,
                Staff = _roadMap.Staff,
                Status = Enums.RoadMapStatus.Completed
            };

            // Act
            _ = _roadMapRepository.UpdateAsync(roadMap).Result;

            // Assert
            Assert.AreEqual(roadMap, _roadMap, "Road map updated a expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_return_error_that_tasks_was_not_provided()
        {
            // Arrange
            RoadMap roadMap = new()
            {
                Id = _roadMap.Id,
                CreatedOn = _roadMap.CreatedOn,
                StaffId = _roadMap.StaffId,
                Tasks = string.Empty,
                Staff = _roadMap.Staff,
                Status = Enums.RoadMapStatus.Completed
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _roadMapRepository.UpdateAsync(roadMap), "Throws exception that tasks is empty");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_delete_road_map_from_db()
        {
            // Act
            _ = _roadMapRepository.DeleteAsync(_roadMap).Result;

            // Assert
            CollectionAssert.DoesNotContain(_dbContext.RoadMaps.ToList(), _roadMap, "Deleted road map as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }
    }
}
