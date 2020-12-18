using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class PositionsRepositoryTest : BaseUnitTest
    {
        public Position _position1;
        public Position _position2;
        public Team _team1;
        public Team _team2;
        public Staff _staff1;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            _team1 = new Team
            {
                Id = 1,
                Name = "test1",
                Description = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime()
            };
            _team2 = new Team
            {
                Id = 2,
                Name = "test2",
                Description = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime()
            };

            _position1 = new Position
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "position1",
                Team = _team1,
                TeamId = _team1.Id
            };
            _position2 = new Position
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "position2"
                ,
                Team = _team2,
                TeamId = _team2.Id
            };
            _staff1 = new Staff
            {
                Id = 1,
                PersonId = 2
            };
            _dbContext.Staff.Add(_staff1);
            _dbContext.Positions.Add(_position1);
            _dbContext.Positions.Add(_position2);
            _dbContext.Teams.Add(_team1);
            _dbContext.Teams.Add(_team2);
            _positionsRepository = new PositionsRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void AddAsync_should_save_position_to_db()
        {
            // Arrange
            Position position = new Position
            {
                Name = "add test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team1,
                TeamId = _team1.Id,
            };

            // act
            int result = _positionsRepository.AddAsync(position).Result;
            Position expected = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);
            // Assert
            Assert.AreEqual(expected, position, "Succesfuly added new position");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_object_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _positionsRepository.AddAsync(null), "Succesfuly throwed exception that position entity is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_name_is_empty()
        {
            // Arrange
            Position position = new Position
            {
                Name = "",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team1,
                TeamId = _team1.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position name is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team2,
                TeamId = 0,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists2()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team1,
                TeamId = 4,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position cannot be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists3()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team2,
                TeamId = 1,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_update_position_into_db()
        {
            // Arrange
            Position position = new Position
            {
                Id = 1,
                Name = "test update",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team2,
                TeamId = _team2.Id,
            };

            // act
            int result = _positionsRepository.UpdateAsync(position).Result;
            Position expected = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);
            // Assert
            Assert.AreEqual(expected, position, "Succesfuly updated position");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_object_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _positionsRepository.UpdateAsync(null), "Succesfuly throwed exception that position entity is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_name_is_empty()
        {
            // Arrange
            Position position = new Position
            {
                Id = 1,
                Name = "",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team1,
                TeamId = _team1.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position name is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists()
        {
            // Arrange
            Position position = new Position
            {
                Id = 1,
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team2,
                TeamId = 0,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists2()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team1,
                TeamId = 4,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists3()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team2,
                TeamId = 1,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void Delete_should_delete_position_from_db()
        {
            // Arrange
            Position position = new Position
            {
                Id = 3,
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team2,
                TeamId = 3,
            };
            _dbContext.Positions.Add(position);
            // Act
            int result = _positionsRepository.DeleteAsync(position).Result;
            Position recieved = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);


            // Assert
            CollectionAssert.AreEqual(new List<Position> { _position1, _position2 }, _dbContext.Positions.ToList(), "Succesfuly deleted from db");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_throw_exception_because_position_relates_to_team()
        {
            // Arrange
            Position position = new Position
            {
                Id = 3,
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Team = _team2,
                TeamId = 1,
            };
            _dbContext.Positions.Add(position);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.DeleteAsync(position), "Should throw exception because of positions relates to team");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void Delete_should_throw_exception_because_position_relates_to_staff()
        {
            // Arrange
            Position position = new Position
            {
                Id = 3,
                Name = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
            };
            _staff1.PositionId = position.Id;
            _dbContext.Positions.Add(position);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.DeleteAsync(position), "Should throw exception because of positions relates to staff");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void Get_should_return_position_from_db()
        {
            // Act
            Position actual = _positionsRepository.Get(_position1.Id);

            // Assert
            Assert.AreEqual(_position1, actual, "Should return position from db");
        }

        [Test]
        public void GetAll_should_return_positions_from_db()
        {
            // Arrange
            List<Position> expected = new List<Position> { _position1, _position2 };
            // Act
            IQueryable<Position> actual = _positionsRepository.GetAll();

            // Assert
            Assert.AreEqual(expected, actual, "Should return positions from db");
        }
    }
}
