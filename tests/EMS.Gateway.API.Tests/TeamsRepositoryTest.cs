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
    public class TeamsRepositoryTest : BaseUnitTest
    {
        public TeamsRepository _repository;
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
            _test1 = new Team
            {
                Id = 1,
                Name = "test1",
                Description = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00)
            };
            _test2 = new Team
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
                Team = _test1,
                TeamId = _test1.Id
            };
            _position2 = new Position
            {
                Id = 2,
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Name = "position2"
                ,
                Team = _test1,
                TeamId = _test1.Id
            };
            _position3 = new Position
            {
                Id = 3,
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Name = "position3",
                Team = _test2,
                TeamId = _test2.Id
            };
            _position4 = new Position
            {
                Id = 4,
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
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
            _repository = new TeamsRepository(_dbContext);
        }

        [Test]
        public void AddAsync_should_add_teams_to_db()
        {
            // Arrange
            Team toAdd = new Team
            {
                Name = "Test",
                Description = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00)
            };

            // Act
            int result = _repository.AddAsync(toAdd).Result;
            Team expected = _dbContext.Teams.FirstOrDefault(t => t.Id == toAdd.Id);

            // Assert
            Assert.AreEqual(expected, toAdd, "Team success add hass been passed");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_an_exception_because_team_name_is_empty()
        {
            // Arrange
            Team toAdd = new Team
            {
                Name = "",
                Description = "test",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00)
            };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(toAdd), "Team succesfully throws an exception because team name is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_throw_an_exception_because_team_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(null), "Team succesfully throws an exception because team name is empty");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void GetAll_should_retrun_all_team_entities()
        {
            // Arrange
            List<Team> expected = new List<Team>() { _test1, _test2 };

            // Act
            IQueryable<Team> teams = _repository.GetAll();

            // Assert
            CollectionAssert.AreEqual(expected, teams, "Should return team entities");
        }

        [Test]
        public void Get_should_retrun_team_enity_by_specified_id()
        {
            // Act
            Team team = _repository.Get(1);

            // Assert
            Assert.AreEqual(_test1, team, "Should return team entity by specified id");
        }

        [Test]
        public void GetPositionsByTEamId_should_retrun_all_position_entites_in_team()
        {
            // Arrange
            List<Position> expected = new List<Position>() { _position1, _position2 };

            // Act
            IQueryable<Position> teams = _repository.GetPositionsByTeamId(1);

            // Assert
            CollectionAssert.AreEqual(expected, teams, "Should return position entities");
        }

        [Test]
        public void UpdateAsync_should_update_enitity_by_id()
        {
            // Arrange
            Team team = new Team
            {
                Id = _test1.Id,
                Name = "new test name",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Description = "new test description"
            };

            // Act
            int result = _repository.UpdateAsync(team).Result;
            Team expected = _dbContext.Teams.FirstOrDefault(team => team.Id == 1);

            // Assert
            Assert.AreEqual(expected.Name, team.Name, "Team name succesfullty update");
            Assert.AreEqual(expected.Description, team.Description, "Team description succesfullty update");
            Assert.AreEqual(expected.CreatedOn, team.CreatedOn, "Team cratedOn succesfullty update");
            Assert.AreEqual(1, expected.Id, "Team with id 1 has been updated");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_an_exception_because_team_name_is_empty()
        {
            // Arrange
            Team team = new Team
            {
                Id = _test1.Id,
                Name = "",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Description = "new test description"
            };

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(team), "Succesfullty throwed an exception that team name cannot be null while updating");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_throw_an_exception_because_team_is_empty()
        {
            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(null), "Succesfullty throwed an exception that team name cannot be null while updating");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_succesfully_delete_team_from_DB()
        {
            // Arrange
            Team team = new Team
            {
                Id = 3,
                Name = "to delete",
                CreatedOn = new DateTime(2020, 01, 01, 12, 00, 00),
                Description = "new test description"
            };
            _dbContext.Teams.Add(team);

            // Act
            int result = _repository.DeleteAsync(team).Result;
            Team deleted = _dbContext.Teams.FirstOrDefault(t => t.Id == team.Id);

            // Assert
            CollectionAssert.AreEqual(new List<Team> { _test1, _test2}, _dbContext.Teams.ToList(), "Succesfullty deleted team");
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
            Assert.ThrowsAsync<DbUpdateException>(() => _repository.DeleteAsync(team), "Succesfullty throwed an exception that team linked with positions");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void DeleteAsync_should_throw_an_exception_because_team_is_null()
        {
            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.DeleteAsync(null), "Succesfullty throwed an exception that team is null");
            _dbContextMock.Verify(a => a.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }
    }
}
