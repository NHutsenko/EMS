using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Core.API.Models;
using EMS.Core.API.Services;
using EMS.Core.API.Tests.Mock;
using EMS.Core.API.Tests.Mocks;
using Google.Protobuf.WellKnownTypes;
using Moq;
using NUnit.Framework;

namespace EMS.Core.API.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class PositionsServiceTests: BaseUnitTest<PositionsService>
    {
        private Position _position1;
        private Position _position2;
        private Team _team;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new PositionsService(null, null, null));

            _team = new Team
            {
                Id = 1,
                Name = "Test",
                CreatedOn = _dateTimeUtil.GetCurrentDateTime()
            };

            _dbContext.Teams.Add(_team);

            _position1 = new Position
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "Test One",
                TeamId = _team.Id,
                HourRate = 10
            };
            _position2 = new Position
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Name = "Test Two",
                TeamId = 2,
                HourRate = 10
            };

            _dbContext.Positions.Add(_position1);
            _dbContext.Positions.Add(_position2);
            _positionsService = new PositionsService(_positionsRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void AddAsync_should_add_position_to_db_via_people_repository()
        {
            // Arrange
            PositionData positionData = new()
			{
				Name = "Test",
				TeamId = _team.Id,
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = 3
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.AddAsync),
				Request = positionData,
				Response = expectedResponse,
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.AddAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Position cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.AddAsync),
				Request = null,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.AddAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            PositionData positionData = new()
			{
				Name = "Test",
				TeamId = 2,
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Cannot add position with non specified team"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.AddAsync),
				Request = positionData,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.AddAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_DbUpdate_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            PositionData positionData = new()
			{
				Name = "Test",
				TeamId = _team.Id,
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while saving position"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.AddAsync),
				Request = positionData,
				Response = new Exception(DbContextMock.ExceptionMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.AddAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            PositionData positionData = new()
			{
				Name = "Test",
				TeamId = _team.Id,
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Position has not been saved"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.AddAsync),
				Request = positionData,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.AddAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_update_position_to_db_via_people_repository()
        {
            // Arrange
            PositionData positionData = new()
			{
				Id = _position1.Id,
				Name = "Test update",
				TeamId = _team.Id,
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = positionData.Id
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.UpdateAsync),
				Request = positionData,
				Response = expectedResponse,
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.UpdateAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Position cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.UpdateAsync),
				Request = null,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.UpdateAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            PositionData positionData = new()
			{
				Id = _position1.Id,
				Name = "Test",
				TeamId = 2,
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Cannot update position with non exists team"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.UpdateAsync),
				Request = positionData,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.UpdateAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_DbUpdate_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            PositionData positionData = new()
			{
				Id = _position1.Id,
				Name = "Test",
				TeamId = _team.Id,
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while updating position"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.UpdateAsync),
				Request = positionData,
				Response = new Exception(DbContextMock.ExceptionMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.UpdateAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            PositionData positionData = new()
			{
				Id = 1,
				Name = "Test",
				TeamId = _team.Id,
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Position has not been updated"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.UpdateAsync),
				Request = positionData,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.UpdateAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_delete_position_from_db()
        {
            // Arrange
            PositionData positionData = new()
			{
				Id = _position2.Id,
				CreatedOn = Timestamp.FromDateTime(_position2.CreatedOn),
				HourRate = _position2.HourRate,
				Name = _position2.Name
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = positionData.Id
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.DeleteAsync),
				Request = positionData,
				Response = expectedResponse,
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.DeleteAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_invalid_operation_exception()
        {
            // Arrange
            PositionData positionData = new()
			{
				Id = _position1.Id,
				CreatedOn = Timestamp.FromDateTime(_position1.CreatedOn),
				HourRate = _position1.HourRate,
				Name = _position1.Name,
				TeamId = _position1.TeamId
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Cannot delete position related to team"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.DeleteAsync),
				Request = positionData,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.DeleteAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Position cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.DeleteAsync),
				Request = null,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.DeleteAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_DbUpdate_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            PositionData positionData = new()
			{
				Id = _position2.Id,
				CreatedOn = Timestamp.FromDateTime(_position2.CreatedOn),
				HourRate = _position2.HourRate,
				Name = _position2.Name,
				TeamId = _position2.TeamId
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while deleting position"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.DeleteAsync),
				Request = positionData,
				Response = new Exception(DbContextMock.ExceptionMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.DeleteAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            PositionData positionData = new()
			{
				Id = _position2.Id,
				CreatedOn = Timestamp.FromDateTime(_position1.CreatedOn),
				HourRate = _position2.HourRate,
				Name = _position2.Name,
				TeamId = _position2.TeamId
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Position has not been deleted"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.DeleteAsync),
				Request = positionData,
				Response = new Exception(expectedResponse.ErrorMessage),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime()
			};

            // Act
            BaseResponse actual = _positionsService.DeleteAsync(positionData, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_return_all_positions_from_db()
        {
            // Arrange
            PositionsResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};

            expectedResponse.Data.Add(new PositionData
            {
                Id = _position1.Id,
                CreatedOn = Timestamp.FromDateTime(_position1.CreatedOn),
                HourRate = _position1.HourRate,
                Name = _position1.Name,
                TeamId = _position1.TeamId
            });
            expectedResponse.Data.Add(new PositionData
            {
                Id = _position2.Id,
                CreatedOn = Timestamp.FromDateTime(_position2.CreatedOn),
                HourRate = _position2.HourRate,
                Name = _position2.Name,
                TeamId = _position2.TeamId
            });
            Empty request = new Empty();
            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            PositionsResponse actual = _positionsService.GetAll(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            PositionsResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.UnknownError, ErrorMessage = "An error occured while loading positions data" }
			};
            Empty request = new Empty();
            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("Test exception")
			};

            // Act
            PositionsResponse actual = _positionsService.GetAll(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetbyId_should_return_position_by_id_from_db()
        {
            // Arrange
            PositionResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty },
				Data = new PositionData { Id = _position2.Id, CreatedOn = Timestamp.FromDateTime(_position2.CreatedOn), HourRate = _position2.HourRate, Name = _position2.Name, TeamId = _position2.TeamId }
			};

            PositionRequest request = new()
			{
				PositionId = _position2.Id
			};
           
            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            PositionResponse actual = _positionsService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetbyId_should_return_not_found_result()
        {
            // Arrange
            PositionResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.DataError, ErrorMessage = "Requested position not found" }
			};

            PositionRequest request = new()
			{
				PositionId = 3
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            PositionResponse actual = _positionsService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetbyId_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            PositionResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.UnknownError, ErrorMessage = "An error occured while loading position data" }
			};

            PositionRequest request = new()
			{
				PositionId = 3
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsService),
				CallerMethodName = nameof(_positionsService.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("Test exception")
			};

            // Act
            PositionResponse actual = _positionsService.GetById(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Data as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
