﻿using System.Diagnostics.CodeAnalysis;
using EMS.Common.Protos;
using EMS.Core.API.Models;
using EMS.Core.API.Services;
using EMS.Core.API.Tests.Mock;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;
using Moq;
using System;
using EMS.Common.Logger.Models;
using EMS.Core.API.Tests.Mocks;

namespace EMS.Core.API.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class TeamsServiceTests : BaseUnitTest<TeamsService>
    {
        private Team _team1;
        private Team _team2;
        private Position _position;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new TeamsService(null, null, null));

            _team1 = new Team
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Id = 1,
                Name = "Team One",
                Description = "Test"
            };

            _team2 = new Team
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Id = 2,
                Name = "Team Two",
                Description = "Test"
            };

            _dbContext.Teams.Add(_team2);
            _dbContext.Teams.Add(_team1);

            _position = new Position
            {
                Id = 1,
                TeamId = _team1.Id
            };
            _dbContext.Positions.Add(_position);

            _teamsService = new TeamsService(_teamsRepository, _dateTimeUtil, _logger);;
        }

        [Test]
        public void AddAsync_should_add_team_to_db_via_teams_repository()
        {
            // Arrange
            TeamData team = new()
			{
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
			};

            BaseResponse expected = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = 3
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = team,
				Response = expected
			};

            // Act
            BaseResponse actual = _teamsService.AddAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Added to DB via TeamsRepository as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expected = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Team cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = null,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.AddAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled null reference exception");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            TeamData team = new()
			{
				Name = "Team One",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
			};

            BaseResponse expected = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Team with the same name already exists"
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = team,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.AddAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled argument exception");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            TeamData team = new()
			{
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
			};

            BaseResponse expected = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while saving team"
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = team,
				Response = new Exception(DbContextMock.ExceptionMessage)
			};

            // Act
            BaseResponse actual = _teamsService.AddAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled DB Update exception");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            TeamData team = new()
			{
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
			};

            BaseResponse expected = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Team has not been saved"
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = team,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.AddAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled exception");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_update_team_to_db_via_teams_repository()
        {
            // Arrange
            TeamData team = new()
			{
				Id = _team1.Id,
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
			};

            BaseResponse expected = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = team.Id
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = team,
				Response = expected
			};

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Added to DB via TeamsRepository as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expected = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Team cannot be empty"
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = null,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled null reference exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            TeamData team = new()
			{
				Id = _team1.Id,
				Name = "Team One",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
			};

            BaseResponse expected = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Team with the same name already exists"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = team,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled argument exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            TeamData team = new()
			{
				Id = _team1.Id,
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
			};

            BaseResponse expected = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while saving team"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = team,
				Response = new Exception(DbContextMock.ExceptionMessage)
			};

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled DB Update exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            TeamData team = new()
			{
				Id = _team1.Id,
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
			};

            BaseResponse expected = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Team has not been updated"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = team,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_delete_team_via_teams_repository()
        {
            // Arrange
            TeamData teamData = new()
			{
				Id = _team2.Id,
				CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
				Name = _team2.Name,
				Description = _team2.Description
			};
            BaseResponse expected = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = teamData.Id
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = teamData,
				Response = expected
			};

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(teamData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Deleted via teams repository");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expected = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Team cannot be empty"
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = null,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled  null reference exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_invalid_operation_exception()
        {
            // Arrange
            BaseResponse expected = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Team cannot be deleted because of this team has positions"
			};
            TeamData teamData = new()
			{
				Id = _team1.Id,
				CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
				Name = _team1.Name,
				Description = _team1.Description
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = teamData,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(teamData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled invalid operation exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            BaseResponse expected = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while deleting team"
			};
            TeamData teamData = new()
			{
				Id = _team2.Id,
				CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
				Name = _team2.Name,
				Description = _team2.Description
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = teamData,
				Response = new Exception(DbContextMock.ExceptionMessage)
			};

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(teamData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled db update exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            BaseResponse expected = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Team has not been deleted"
			};
            TeamData teamData = new()
			{
				Id = _team2.Id,
				CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
				Name = _team2.Name,
				Description = _team2.Description
			};
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = teamData,
				Response = new Exception(expected.ErrorMessage)
			};

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(teamData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_return_team_data()
        {
            // Arrange
            TeamRequest request = new()
			{
				Id = _team1.Id
			};

            TeamResponse expected = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty },
				Data = new TeamData { Id = _team1.Id, CreatedOn = Timestamp.FromDateTime(_team1.CreatedOn.ToUniversalTime()), Description = _team1.Description, Name = _team1.Name }
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expected
			};

            // Act
            TeamResponse actual = _teamsService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Team data returned as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_handle_not_found_team_response_from_repository()
        {
            // Arrange
            TeamRequest request = new()
			{
				Id = 3
			};

            TeamResponse expected = new()
			{
				Status = new BaseResponse { Code = Code.DataError, ErrorMessage = "Requested team not found" }
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expected
			};

            // Act
            TeamResponse actual = _teamsService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Not found team response handled");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            TeamRequest request = new()
			{
				Id = 3
			};

            TeamResponse expected = new()
			{
				Status = new BaseResponse { Code = Code.UnknownError, ErrorMessage = "An error occured while loading team data" }
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("Test exception")
			};

            // Act
            TeamResponse actual = _teamsService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Excpetion handled");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_return_all_teams()
        {
            // Arrange
            TeamsResponse expected = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};

            expected.Data.Add(new TeamData
            {
                Id = _team2.Id,
                CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
                Description = _team2.Description,
                Name = _team2.Name
            });

            expected.Data.Add(new TeamData
            {
                Id = _team1.Id,
                CreatedOn = Timestamp.FromDateTime(_team1.CreatedOn.ToUniversalTime()),
                Description = _team1.Description,
                Name = _team1.Name
            });

            Empty request = new Empty();

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expected
			};

            // Act
            TeamsResponse actual = _teamsService.GetAll(request, null).Result;

            // Assert
            Assert.AreEqual(expected.Status, actual.Status, "Response status as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Response status as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            TeamsResponse expected = new()
			{
				Status = new BaseResponse { Code = Code.UnknownError, ErrorMessage = "An error occured while loading teams data" }
			};
            Empty request = new Empty();
            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsService),
				CallerMethodName = nameof(_teamsService.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("Test exception")
			};

            // Act
            TeamsResponse actual = _teamsService.GetAll(request, null).Result;

            // Assert
            Assert.AreEqual(expected.Status, actual.Status, "Response status as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Response status as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
