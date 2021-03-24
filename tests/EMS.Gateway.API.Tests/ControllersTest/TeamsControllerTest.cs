using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Gateway.API.Controllers;
using EMS.Gateway.API.Tests.Mock;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EMS.Gateway.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class TeamsControllerTest : BaseUnitTest<TeamsController>
    {
        private TeamsController _teamsController;

        [SetUp]

        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new TeamsController(null, null, null));
            _teamsController = new TeamsController(_teamsClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void Add_should_return_response_from_client()
        {
            // Arrange
            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };
            BaseMock.Response = response;
            TeamData request = new()
			{
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime())
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.Add),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = response
			};

            // Act
            ObjectResult actual = _teamsController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;
            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            TeamData request = new()
            {
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime())
            };

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.Add),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _teamsController.Add(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_return_response_from_client()
        {
            // Arrange
            BaseResponse response = new()
			{
				Code = Code.Success,
				DataId = 1,
				ErrorMessage = string.Empty
			};
            BaseMock.Response = response;
            TeamData request = new()
			{
				Id = 1,
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime())
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.Update),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = response
			};

            // Act
            ObjectResult actual = _teamsController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;
            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            TeamData request = new()
            {
                Id = 1,
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime())
            };

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.Update),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _teamsController.Update(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_return_response_from_client()
        {
            // Arrange
            BaseResponse response = new()
			{
				Code = Code.Success,
				DataId = 1,
				ErrorMessage = string.Empty
			};
            BaseMock.Response = response;
            TeamData request = new()
			{
				Id = 1,
				Name = "test",
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime())
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.Delete),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = response
			};

            // Act
            ObjectResult actual = _teamsController.Delete(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;
            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            TeamData request = new()
            {
                Id = 1,
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime())
            };

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.Delete),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _teamsController.Delete(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetAll_should_return_response_from_grpc_client()
        {
            // Arrange
            TeamsResponse response = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};
            response.Data.Add(new TeamData
            {
                Id = 1,
                Name = "test",
                Description = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime())
            });
            BaseMock.Response = response;

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = new Empty(),
				Response = response
			};

            // Act
            ObjectResult actual = _teamsController.GetAll() as ObjectResult;
            TeamsResponse actualData = actual.Value as TeamsResponse;
            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.GetAll(new Empty(), null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetAll_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = new Empty(),
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _teamsController.GetAll() as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.GetAll(new Empty(), null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetById_should_return_response_from_grpc_client()
        {
            // Arrange
            TeamRequest request = new()
			{
				Id = 1
			};
            TeamResponse response = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty },
				Data = new TeamData { Id = 1, Name = "test", Description = "test", CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()) }
			};
            BaseMock.Response = response;

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = response
			};

            // Act
            ObjectResult actual = _teamsController.GetById(request.Id) as ObjectResult;
            TeamResponse actualData = actual.Value as TeamResponse;
            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.GetById(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetById_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            TeamRequest request = new()
			{
				Id = 1
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(TeamsController),
				CallerMethodName = nameof(_teamsController.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _teamsController.GetById(request.Id) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _teamsClientMock.Verify(m => m.GetById(request, null, null, new CancellationToken()), Times.Once);
        }
    }
}
