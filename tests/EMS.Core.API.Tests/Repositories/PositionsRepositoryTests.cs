﻿using System;
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
    public class PositionsRepositoryTests : BaseUnitTest<PositionsRepository>
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
            DbContextMock.ShouldThrowException = false;

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
            Position position = new()
			{
				Name = "add test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = _team1.Id,
				HourRate = 10
			};

            // Act
            int result = _positionsRepository.AddAsync(position).Result;
            Position expected = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);
            // Assert
            Assert.AreEqual(expected, position, "Succesfuly added new position");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Position position = new()
			{
				Name = "add test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = _team1.Id,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.AddAsync(position), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_object_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _positionsRepository.AddAsync(null), "Succesfuly throwed exception that position entity is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_name_is_empty()
        {
            // Arrange
            Position position = new()
			{
				Name = "",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = _team1.Id,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position name is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_argument_exception_because_position_with_the_same_name_exists_in_db()
        {
            // Arrange
            Position position = new()
			{
				Name = "position1",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = _team1.Id,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position name is exists in DB");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_wrong_hour_rate()
        {
            // Arrange
            Position position = new()
			{
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team2,
				TeamId = 0
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position cant be added with wrong hour rate");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }


        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists()
        {
            // Arrange
            Position position = new()
			{
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team2,
				TeamId = 0,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists2()
        {
            // Arrange
            Position position = new()
			{
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = 4,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.AddAsync(position), "Succesfuly throwed exception that position cannot be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_update_position_into_db()
        {
            // Arrange
            Position position = new()
			{
				Id = 1,
				Name = "test update",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team2,
				TeamId = _team2.Id,
				HourRate = 10
			};

            // act
            int result = _positionsRepository.UpdateAsync(position).Result;
            Position expected = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);
            // Assert
            Assert.AreEqual(expected, position, "Succesfuly updated position");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Position position = new()
			{
				Name = "add test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = _team1.Id,
				Id = _position1.Id,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.UpdateAsync(position), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_object_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _positionsRepository.UpdateAsync(null), "Succesfuly throwed exception that position entity is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_name_is_empty()
        {
            // Arrange
            Position position = new()
			{
				Id = 1,
				Name = "",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = _team1.Id
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position name is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_argument_exception_because_position_with_the_same_name_exists_in_db()
        {
            // Arrange
            Position position = new()
			{
				Id = 1,
				Name = "position1",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = _team1.Id,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position name is exists in DB");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_wrong_hour_rate()
        {
            // Arrange
            Position position = new()
			{
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team2,
				TeamId = 0,
				Id = 1
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with wrong hour rate");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists()
        {
            // Arrange
            Position position = new()
			{
				Id = 1,
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team2,
				TeamId = 0,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throws_an_exception_that_position_cant_be_added_with_team_that_is_not_exists2()
        {
            // Arrange
            Position position = new()
			{
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = 4,
				HourRate = 10
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _positionsRepository.UpdateAsync(position), "Succesfuly throwed exception that position cant be added with team that is not exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_Delete_position_from_db()
        {
            // Arrange
            Position position = new()
			{
				Id = 3,
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team2,
				TeamId = 3
			};
            _dbContext.Positions.Add(position);
            // Act
            int result = _positionsRepository.DeleteAsync(position).Result;
            Position recieved = _dbContext.Positions.FirstOrDefault(p => p.Id == position.Id);


            // Assert
            CollectionAssert.AreEqual(new List<Position> { _position1, _position2 }, _dbContext.Positions.ToList(), "Succesfuly Deletefrom db");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Position position = new()
			{
				Name = "add test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team1,
				TeamId = 3,
				Id = _position1.Id
			};

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _positionsRepository.DeleteAsync(position), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_throws_an_exception_that_position_object_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _positionsRepository.DeleteAsync(null), "Succesfuly throwed exception that position entity is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_throw_exception_because_position_relates_to_team()
        {
            // Arrange
            Position position = new()
			{
				Id = 3,
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Team = _team2,
				TeamId = 1
			};
            _dbContext.Positions.Add(position);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _positionsRepository.DeleteAsync(position), "Should throw exception because of positions relates to team");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_throw_exception_because_position_relates_to_staff()
        {
            // Arrange
            Position position = new()
			{
				Id = 3,
				Name = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};
            _staff1.PositionId = position.Id;
            _dbContext.Positions.Add(position);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _positionsRepository.DeleteAsync(position), "Should throw exception because of positions relates to staff");
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
