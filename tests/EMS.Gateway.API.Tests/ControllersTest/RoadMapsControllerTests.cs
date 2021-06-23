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
    public class RoadMapsControllerTests : BaseUnitTest<RoadMapsController>
    {
        private RoadMapsController _controller;
        private RoadMapData _roadMapData;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new RoadMapsController(null, null, null));

            _roadMapData = new RoadMapData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Id = 1,
                StaffId = 1,
                Status = 1,
                Tasks = "test"
            };
            _controller = new RoadMapsController(_roadMapsClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetByStaffId_should_return_road_map_by_staff_id()
        {
            // Arrange
            ByStaffRequest request = new ByStaffRequest
            {
                StaffId = _roadMapData.StaffId
            };
            RoadMapResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                },
                Data = _roadMapData
            };
            BaseMock.Response = response;
            LogData log = new()
            {
                CallSide = nameof(RoadMapsController),
                CallerMethodName = nameof(_controller.GetByStaffId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            OkObjectResult result = _controller.GetByStaffId(request) as OkObjectResult;
            RoadMapResponse actual = result.Value as RoadMapResponse;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(log), Times.Once);
            _roadMapsClientMock.Verify(m => m.GetByStaffId(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetByStaffId_should_handle_exception()
        {
            // Arrange
            ByStaffRequest request = new ByStaffRequest
            {
                StaffId = _roadMapData.StaffId
            };
            RoadMapResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = BaseMock.ExceptionMessage,
                }
            };
            BaseMock.ShouldThrowException = true;
            LogData log = new()
            {
                CallSide = nameof(RoadMapsController),
                CallerMethodName = nameof(_controller.GetByStaffId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult result = _controller.GetByStaffId(request) as ObjectResult;

            // Assert
            Assert.AreEqual(result.Value, BaseMock.ErrorResponseMessage, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(log), Times.Once);
            _roadMapsClientMock.Verify(m => m.GetByStaffId(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Add_should_add_road_map_by_staff_id()
        {
            // Arrange
            RoadMapData request = new()
            {
                StaffId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Status = 1,
                Tasks = "test"
            };
            BaseResponse response = new()
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = 2,
            };
            BaseMock.Response = response;
            LogData log = new()
            {
                CallSide = nameof(RoadMapsController),
                CallerMethodName = nameof(_controller.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            OkObjectResult result = _controller.Add(request) as OkObjectResult;
            BaseResponse actual = result.Value as BaseResponse;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(log), Times.Once);
            _roadMapsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            RoadMapData request = new()
            {
                StaffId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Status = 1,
                Tasks = "test"
            };
            BaseResponse response = new()
            {
                Code = Code.UnknownError,
                ErrorMessage = BaseMock.ExceptionMessage,
            };
            BaseMock.ShouldThrowException = true;
            LogData log = new()
            {
                CallSide = nameof(RoadMapsController),
                CallerMethodName = nameof(_controller.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult result = _controller.Add(request) as ObjectResult;

            // Assert
            Assert.AreEqual(result.Value, BaseMock.ErrorResponseMessage, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(log), Times.Once);
            _roadMapsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_update_road_map_by_id()
        {
            // Arrange
            RoadMapData request = new()
            {
                Id = 1,
                StaffId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Status = 1,
                Tasks = "test"
            };
            BaseResponse response = new()
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = 2,
            };
            BaseMock.Response = response;
            LogData log = new()
            {
                CallSide = nameof(RoadMapsController),
                CallerMethodName = nameof(_controller.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            OkObjectResult result = _controller.Update(request) as OkObjectResult;
            BaseResponse actual = result.Value as BaseResponse;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(log), Times.Once);
            _roadMapsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            RoadMapData request = new()
            {
                StaffId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Status = 1,
                Tasks = "test"
            };
            BaseResponse response = new()
            {
                Code = Code.UnknownError,
                ErrorMessage = BaseMock.ExceptionMessage,
            };
            BaseMock.ShouldThrowException = true;
            LogData log = new()
            {
                CallSide = nameof(RoadMapsController),
                CallerMethodName = nameof(_controller.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult result = (ObjectResult)_controller.Update(request);

            // Assert
            Assert.AreEqual(result.Value, BaseMock.ErrorResponseMessage, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(log), Times.Once);
            _roadMapsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_delete_road_map_by_id()
        {
            // Arrange
            RoadMapData request = new()
            {
                Id = 1,
                StaffId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Status = 1,
                Tasks = "test"
            };
            BaseResponse response = new()
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = 2,
            };
            BaseMock.Response = response;
            LogData log = new()
            {
                CallSide = nameof(RoadMapsController),
                CallerMethodName = nameof(_controller.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            OkObjectResult result = _controller.Delete(request) as OkObjectResult;
            BaseResponse actual = result.Value as BaseResponse;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(log), Times.Once);
            _roadMapsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_handle_exception()
        {
            // Arrange
            RoadMapData request = new()
            {
                StaffId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Status = 1,
                Tasks = "test"
            };
            BaseResponse response = new()
            {
                Code = Code.UnknownError,
                ErrorMessage = BaseMock.ExceptionMessage,
            };
            BaseMock.ShouldThrowException = true;
            LogData log = new()
            {
                CallSide = nameof(RoadMapsController),
                CallerMethodName = nameof(_controller.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult result = (ObjectResult)_controller.Delete(request);

            // Assert
            Assert.AreEqual(result.Value, BaseMock.ErrorResponseMessage, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(log), Times.Once);
            _roadMapsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
        }
    }
}
