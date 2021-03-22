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
    public class PositionsControllerTest : BaseUnitTest<PositionsController>
    {
        private PositionsController _positionsController;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new PositionsController(null, null, null));
            _positionsController = new PositionsController(_positionsClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void Add_should_return_result_from_grpc_client()
        {
            // Arrange
            PositionData request = new()
            {
                Name = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                HourRate = 10,
                TeamId = 1
            };

            BaseResponse response = new()
			{
				Code = Code.Success,
				DataId = 1,
				ErrorMessage = string.Empty
			};

            BaseMock.Response = response;

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.Add),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = response
			};

            // Act
            ObjectResult actual = _positionsController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Asssert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actualData, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            PositionData request = new()
			{
				Name = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10,
				TeamId = 1
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.Add),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _positionsController.Add(request) as ObjectResult;

            // Asssert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_return_result_from_grpc_client()
        {
            // Arrange
            PositionData request = new()
			{
				Name = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10,
				TeamId = 1,
				Id = 1
			};

            BaseResponse response = new()
			{
				Code = Code.Success,
				DataId = 1,
				ErrorMessage = string.Empty
			};

            BaseMock.Response = response;

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.Update),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = response
			};

            // Act
            ObjectResult actual = _positionsController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Asssert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actualData, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            PositionData request = new()
			{
				Name = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10,
				TeamId = 1
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.Update),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _positionsController.Update(request) as ObjectResult;

            // Asssert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_return_result_from_grpc_client()
        {
            // Arrange
            PositionData request = new()
			{
				Name = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10,
				TeamId = 1,
				Id = 1
			};

            BaseResponse response = new()
			{
				Code = Code.Success,
				DataId = 1,
				ErrorMessage = string.Empty
			};

            BaseMock.Response = response;

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.Delete),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = response
			};

            // Act
            ObjectResult actual = _positionsController.Delete(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Asssert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actualData, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            PositionData request = new()
			{
				Name = "test",
				CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
				HourRate = 10,
				TeamId = 1,
				Id = 1
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.Delete),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _positionsController.Delete(request) as ObjectResult;

            // Asssert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetAll_should_return_Response_from_grpc_client()
        {
            // Arrange
            PositionsResponse response = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};
            response.Data.Add(new PositionData
            {
                Name = "test",
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                HourRate = 10,
                TeamId = 1,
                Id = 1
            });
            BaseMock.Response = response;
            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = new Empty(),
				Response = response
			};

            // Act
            ObjectResult actual = _positionsController.GetAll() as ObjectResult;
            PositionsResponse actualData = actual.Value as PositionsResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actual.Value, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.GetAll(new Empty(), null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetAll_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = new Empty(),
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _positionsController.GetAll() as ObjectResult;
            PositionsResponse actualData = actual.Value as PositionsResponse;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.GetAll(new Empty(), null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetById_should_return_response_from_rpc_client()
        {
            // Arrange
            PositionResponse response = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty },
				Data = new PositionData { Name = "test", CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()), HourRate = 10, TeamId = 1, Id = 1 }
			};
            BaseMock.Response = response;
            PositionRequest request = new()
			{
				PositionId = 1
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = response
			};

            // Act
            ObjectResult actual = _positionsController.GetById(request.PositionId) as ObjectResult;
            PositionResponse actualData = actual.Value as PositionResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actual.Value, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.GetById(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetById_should_handle_exception()
        {
			// Arrange
			BaseMock.ShouldThrowException = true;
            PositionRequest request = new()
			{
				PositionId = 1
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(PositionsController),
				CallerMethodName = nameof(_positionsController.GetById),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(BaseMock.ExceptionMessage)
			};

            // Act
            ObjectResult actual = _positionsController.GetById(request.PositionId) as ObjectResult;
            PositionResponse actualData = actual.Value as PositionResponse;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _positionsClientMock.Verify(m => m.GetById(request, null, null, new CancellationToken()), Times.Once);
        }
    }
}
