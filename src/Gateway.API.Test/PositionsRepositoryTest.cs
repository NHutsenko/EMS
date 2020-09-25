using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMS.Gateway.API.Models;
using EMS.Gateway.API.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Gateway.API.Test
{
    public class PositionsRepositoryTest : BaseUnitTest
    {
        public PositionsRepository _repository;
        public Position _position1;
        public Position _position2;
        public Team _team1;
        public Team _team2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            _team1 = new Team
            {
                Id = 1,
                Name = "test1",
                Description = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00)
            };
            _team2 = new Team
            {
                Id = 2,
                Name = "test2",
                Description = "test",
                CreatedOn = new DateTime(2020, 01, 01, 13, 00, 00)
            };

            _position1 = new Position
            {
                Id = 1,
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Name = "position1",
                Team = _team1,
                TeamId = _team1.Id
            };
            _position2 = new Position
            {
                Id = 2,
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Name = "position2"
                ,
                Team = _team2,
                TeamId = _team2.Id
            };
            _dbContext.Positions.Add(_position1);
            _dbContext.Positions.Add(_position2);
            _dbContext.Teams.Add(_team1);
            _dbContext.Teams.Add(_team2);
            _repository = new PositionsRepository(_dbContext);
        }

        [Test]
        public void AddAsync_should_save_position_to_db()
        {
            // Arrange
            Position position = new Position
            {
                Name = "add test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team1,
                TeamId = _team1.Id,
            };

            // act
            int result = _repository.AddAsync(position).Result;
            Position expected = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);
            // Assert
            Assert.AreEqual(expected, position, "Succesfuly added new position");
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_object_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(null), "Succesfuly throwed exception that position entity is empty");
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_name_is_empty()
        {
            // Arrange
            Position position = new Position
            {
                Name = "",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team1,
                TeamId = _team1.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(null), "Succesfuly throwed exception that position name is empty");
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team2,
                TeamId = 0,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists2()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team1,
                TeamId = 4,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists3()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team2,
                TeamId = 1,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
        }
        public void UpdateAsync_should_update_position_into_db()
        {
            // Arrange
            Position position = new Position
            {
                Id = 1,
                Name = "test update",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team2,
                TeamId = _team2.Id,
            };

            // act
            int result = _repository.UpdateAsync(position).Result;
            Position expected = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);
            // Assert
            Assert.AreEqual(expected, position, "Succesfuly added new position");
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_object_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(null), "Succesfuly throwed exception that position entity is empty");
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_name_is_empty()
        {
            // Arrange
            Position position = new Position
            {
                Id = 1,
                Name = "",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team1,
                TeamId = _team1.Id,
            };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(position), "Succesfuly throwed exception that position name is empty");
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists()
        {
            // Arrange
            Position position = new Position
            {
                Id = 1,
                Name = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team2,
                TeamId = 0,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _repository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists2()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team1,
                TeamId = 4,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _repository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists3()
        {
            // Arrange
            Position position = new Position
            {
                Name = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team2,
                TeamId = 1,
            };

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _repository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
        }

        [Test]
        public void Delete_should_delete_team_from_db()
        {
            // Arrange
            Position position = new Position
            {
                Id = 3,
                Name = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Team = _team2,
                TeamId = 1,
            };
            _dbContext.Positions.Add(position);
            // Act
            int result = _repository.DeleteAsync(position).Result;
            Position recieved = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);


            // Assert
            Assert.AreEqual(null, recieved, "Succesfuly deleted from db");
        }
    }
}
