using System.Diagnostics.CodeAnalysis;
using EMS.Common.Protos;
using EMS.Core.API.Models;
using EMS.Core.API.Services;
using EMS.Core.API.Tests.Mock;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;
using EMS.Common.Models.BaseModel;
using Moq;
using System;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class TeamsServiceTest : BaseUnitTest<TeamsService>
    {
        private Team _team1;
        private Team _team2;
        private Position _position;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            DbContextMock.ShouldThrowException = false;
            DbContextMock.SaveChangesResult = 1;

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

            _teamsRepository = new DAL.Repositories.TeamsRepository(_dbContext, _dateTimeUtil);
            InitializeLoggerMock(new TeamsService(null, null, null));
            _teamsService = new TeamsService(_teamsRepository, _dateTimeUtil, _logger);;
        }

        [Test]
        public void AddAsync_should_add_team_to_db_via_teams_repository()
        {
            // Arrange
            TeamData team = new TeamData
            {
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = team,
                Response = expected
            };

            // Act
            BaseResponse actual = _teamsService.AddAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Added to DB via TeamsRepository as expected");
            _loggerMock.Verify(m => m.AddLog(expectedRrObject), "OperationLogged");
        }

        [Test]
        public void AddAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Team cannot be empty"
            };

            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.AddAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled null reference exception");
            _loggerMock.Verify(m => m.AddErrorLog(expectedRrObject), "OperationLogged");
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            TeamData team = new TeamData
            {
                Name = "Team One",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Team with the same name already exists"
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = team,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.AddAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled argument exception");
            _loggerMock.Verify(m => m.AddErrorLog(expectedRrObject), "OperationLogged");
        }

        [Test]
        public void AddAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            TeamData team = new TeamData
            {
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving team"
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = team,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _teamsService.AddAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled DB Update exception");
            _loggerMock.Verify(m => m.AddErrorLog(expectedRrObject), "OperationLogged");
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            TeamData team = new TeamData
            {
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Team has not been saved"
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = team,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.AddAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled exception");
            _loggerMock.Verify(m => m.AddErrorLog(expectedRrObject), "OperationLogged");
        }

        [Test]
        public void UpdateAsync_should_update_team_to_db_via_teams_repository()
        {
            // Arrange
            TeamData team = new TeamData
            {
                Id = _team1.Id,
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty
            };

            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = team,
                Response = expected
            };

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Added to DB via TeamsRepository as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void UpdateAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Team cannot be empty"
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled null reference exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            TeamData team = new TeamData
            {
                Id = _team1.Id,
                Name = "Team One",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Team with the same name already exists"
            };

            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = team,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled argument exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void UpdateAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            TeamData team = new TeamData
            {
                Id = _team1.Id,
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving team"
            };

            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = team,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled DB Update exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            TeamData team = new TeamData
            {
                Id = _team1.Id,
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            BaseResponse expected = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Team has not been updated"
            };

            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = team,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.UpdateAsync(team, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void DeleteAsync_should_delete_team_via_teams_repository()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty
            };
            TeamData teamData = new TeamData
            {
                Id = _team2.Id,
                CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
                Name = _team2.Name,
                Description = _team2.Description
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = teamData,
                Response = expected
            };

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(teamData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Deleted via teams repository");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void DeleteAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Team cannot be empty"
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled  null reference exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void DeleteAsync_should_handle_invalid_operation_exception()
        {
            // Arrange
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Team cannot be deleted because of this team has positions"
            };
            TeamData teamData = new TeamData
            {
                Id = _team1.Id,
                CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
                Name = _team1.Name,
                Description = _team1.Description
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = teamData,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(teamData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled invalid operation exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void DeleteAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            BaseResponse expected = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while deleting team"
            };
            TeamData teamData = new TeamData
            {
                Id = _team2.Id,
                CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
                Name = _team2.Name,
                Description = _team2.Description
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = teamData,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(teamData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled db update exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void DeleteAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            BaseResponse expected = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Team has not been deleted"
            };
            TeamData teamData = new TeamData
            {
                Id = _team2.Id,
                CreatedOn = Timestamp.FromDateTime(_team2.CreatedOn.ToUniversalTime()),
                Name = _team2.Name,
                Description = _team2.Description
            };
            RequestResponseObject expectedRrObject = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = teamData,
                Response = new Exception(expected.ErrorMessage)
            };

            // Act
            BaseResponse actual = _teamsService.DeleteAsync(teamData, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Handled exception");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedRrObject), "Data logged");
        }

        [Test]
        public void GetById_should_return_team_data()
        {
            // Arrange
            TeamRequest request = new TeamRequest
            {
                Id = _team1.Id
            };

            TeamResponse expected = new TeamResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                },
                Data = new TeamData
                {
                    Id = _team1.Id,
                    CreatedOn = Timestamp.FromDateTime(_team1.CreatedOn.ToUniversalTime()),
                    Description = _team1.Description,
                    Name = _team1.Name
                }
            };

            RequestResponseObject expectedRequestResponse = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expected
            };

            // Act
            TeamResponse actual = _teamsService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Team data returned as expected");
            _loggerMock.Verify(m => m.AddLog(expectedRequestResponse), "Data logged");
        }

        [Test]
        public void GetById_should_handle_not_found_team_response_from_repository()
        {
            // Arrange
            TeamRequest request = new TeamRequest
            {
                Id = 3
            };

            TeamResponse expected = new TeamResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = "Requested team not found"
                }
            };

            RequestResponseObject expectedRequestResponse = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expected
            };

            // Act
            TeamResponse actual = _teamsService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expected, actual, "Not found team response handled");
            _loggerMock.Verify(m => m.AddLog(expectedRequestResponse), "Data logged");
        }

        [Test]
        public void GetAll_should_return_all_teams()
        {
            // Arrange
            TeamsResponse expected = new TeamsResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
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

            RequestResponseObject expectedRequestResponse = new RequestResponseObject
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expected
            };

            // Act
            TeamsResponse actual = _teamsService.GetAll(request, null).Result;

            // Assert
            Assert.AreEqual(expected.Response, actual.Response, "Response status as expected");
            Assert.AreEqual(expected.Data, actual.Data, "Response status as expected");
            _loggerMock.Verify(m => m.AddLog(expectedRequestResponse), "Data logged");
        }
    }
}
