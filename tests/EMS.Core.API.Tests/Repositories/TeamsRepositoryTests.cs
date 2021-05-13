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
    public class TeamsRepositoryTests : BaseUnitTest<TeamsRepository>
    {
        public Team _test1;
        public Team _test2;
        public Position _position1;
        public Position _position2;
        public Position _position3;
        public Position _position4;


        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            DbContextMock.ShouldThrowException = false;

            _test1 = new Team
            {
                Id = 1,
                Name = "test1",
                Description = "test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime()
            };
            _test2 = new Team
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
                Team = _test1,
                TeamId = _test1.Id
            };
            _position2 = new Position
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "position2"
                ,
                Team = _test1,
                TeamId = _test1.Id
            };
            _position3 = new Position
            {
                Id = 3,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "position3",
                Team = _test2,
                TeamId = _test2.Id
            };
            _position4 = new Position
            {
                Id = 4,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "position4",
                Team = _test2,
                TeamId = _test2.Id
            };
            _dbContext.Positions.Add(_position1);
            _dbContext.Positions.Add(_position2);
            _dbContext.Positions.Add(_position3);
            _dbContext.Positions.Add(_position4);
            _dbContext.Teams.Add(_test1);
            _dbContext.Teams.Add(_test2);
            _teamsRepository = new TeamsRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void AddAsync_should_add_teams_to_db()
        {
            // Arrange
            Team toAdd = new()
			{
				Name = "Test",
				Description = "test"
			};

            // Act
            int result = _teamsRepository.AddAsync(toAdd).Result;
            Team expected = _dbContext.Teams.FirstOrDefault(t => t.Id == toAdd.Id);

            // Assert
            Assert.AreEqual(expected, toAdd, "Team success add hass been passed");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_argument_exception_because_team_with_the_same_name_already_exists()
        {
            // Arrange
            Team toAdd = new()
			{
				Name = "test1",
				Description = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _teamsRepository.AddAsync(toAdd), "Team succesfully throws an exception because team with the same name already exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_an_exception_because_team_name_is_empty()
        {
            // Arrange
            Team toAdd = new()
			{
				Name = "",
				Description = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _teamsRepository.AddAsync(toAdd), "Team succesfully throws an exception because team name is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Team toAdd = new()
			{
				Name = "test",
				Description = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _teamsRepository.AddAsync(toAdd), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_an_exception_because_team_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _teamsRepository.AddAsync(null), "Team succesfully throws an exception because team name is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void GetAll_should_retrun_all_team_entities()
        {
            // Arrange
            List<Team> expected = new List<Team>() { _test1, _test2 };

            // Act
            IQueryable<Team> teams = _teamsRepository.GetAll();

            // Assert
            CollectionAssert.AreEqual(expected, teams, "Should return team entities");
        }

        [Test]
        public void Get_should_retrun_team_enity_by_specified_id()
        {
            // Act
            Team team = _teamsRepository.Get(1);

            // Assert
            Assert.AreEqual(_test1, team, "Should return team entity by specified id");
        }

        [Test]
        public void UpdateAsync_should_update_enitity_by_id()
        {
            // Arrange
            Team team = new()
			{
				Id = _test1.Id,
				Name = "new test name",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Description = "new test description"
			};

            // Act
            int result = _teamsRepository.UpdateAsync(team).Result;
            Team expected = _dbContext.Teams.FirstOrDefault(team => team.Id == 1);

            // Assert
            Assert.AreEqual(expected.Name, team.Name, "Team name succesfullty update");
            Assert.AreEqual(expected.Description, team.Description, "Team description succesfullty update");
            Assert.AreEqual(expected.CreatedOn, team.CreatedOn, "Team cratedOn succesfullty update");
            Assert.AreEqual(1, expected.Id, "Team with id 1 has been updated");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_argument_exception_because_team_with_the_same_name_already_exists()
        {
            // Arrange
            Team toAdd = new()
			{
				Id = _test2.Id,
				Name = "test1",
				Description = "test",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _teamsRepository.UpdateAsync(toAdd), "Team succesfully throws an exception because team with the same name already exists");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Team team = new()
			{
				Id = _test1.Id,
				Name = "new test name",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Description = "new test description"
			};

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _teamsRepository.UpdateAsync(team), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_an_exception_because_team_name_is_empty()
        {
            // Arrange
            Team team = new()
			{
				Id = _test1.Id,
				Name = "",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Description = "new test description"
			};

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _teamsRepository.UpdateAsync(team), "Succesfullty throwed an exception that team name cannot be null while updating");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_an_exception_because_team_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _teamsRepository.UpdateAsync(null), "Succesfullty throwed an exception that team name cannot be null while updating");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_succesfully_delete_team_from_DB()
        {
            // Arrange
            Team team = new()
			{
				Id = 3,
				Name = "to delete",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Description = "new test description"
			};
            _dbContext.Teams.Add(team);

            // Act
            int result = _teamsRepository.DeleteAsync(team).Result;
            Team deleted = _dbContext.Teams.FirstOrDefault(t => t.Id == team.Id);

            // Assert
            CollectionAssert.AreEqual(new List<Team> { _test1, _test2}, _dbContext.Teams.ToList(), "Succesfullty deleted team");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_throw_exception_from_db()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            Team team = new()
			{
				Id = 3,
				Name = "new test name",
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Description = "new test description"
			};

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(() => _teamsRepository.DeleteAsync(team), "Exception from db throws as expected");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_throw_an_exception_because_team_is_using_in_links_with_position()
        {
            // Arrange
            Team team = new Team
            {
                Id = 1
            };

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _teamsRepository.DeleteAsync(team), "Succesfullty throwed an exception that team linked with positions");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_throw_an_exception_because_team_is_null()
        {
            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _teamsRepository.DeleteAsync(null), "Succesfullty throwed an exception that team is null");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }
    }
}
