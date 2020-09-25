using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMS.Gateway.API.Models;
using EMS.Gateway.API.Repositories;
using NUnit.Framework;

namespace Gateway.API.Test
{
    public class TeamsRepositoryTest: BaseUnitTest
    {
        public TeamsRepository _repository;
        public Team _test1;
        public Team _test2;


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
        public void GetAll_should_retrun_team_enity_by_specified_id()
        {
            // Act
            Team team = _repository.Get(1);

            // Assert
            Assert.AreEqual(_test1, team, "Should return team entity by specified id");
        }
    }
}
