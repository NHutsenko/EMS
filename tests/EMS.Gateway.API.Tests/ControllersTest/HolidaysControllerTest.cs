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
    public class HolidaysControllerTest: BaseUnitTest<HolidaysController>
    {
        private HolidaysController _holidaysController;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new HolidaysController(null, null, null));

            _holidaysController = new HolidaysController(_holidaysClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void Add_should_return_response_from_grpc_client()
        {
            // Arrange
            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };
            BaseMock.Response = response;

            HolidayData request = new()
            {
                HolidayDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Description = "test",
                ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _holidaysController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _holidaysClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;

            HolidayData request = new()
            {
                HolidayDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Description = "test",
                ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _holidaysController.Add(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _holidaysClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void Update_should_return_response_from_grpc_client()
        {
            // Arrange
            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };
            BaseMock.Response = response;

            HolidayData request = new()
            {
                Id = 1,
                HolidayDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Description = "test",
                ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _holidaysController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _holidaysClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;

            HolidayData request = new()
            {
                HolidayDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Description = "test",
                ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _holidaysController.Update(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _holidaysClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void Delete_should_return_response_from_grpc_client()
        {
            // Arrange
            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };
            BaseMock.Response = response;

            HolidayData request = new()
            {
                Id = 1,
                HolidayDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Description = "test",
                ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _holidaysController.Delete(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _holidaysClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Delete_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;

            HolidayData request = new()
            {
                HolidayDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Description = "test",
                ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _holidaysController.Delete(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _holidaysClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_return_responce_from_grpc_client()
        {
            // Arrange
            HolidaysResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data.Add(new HolidayData
            {
                Id = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Description = "test",
                HolidayDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            });
            BaseMock.Response = response;
            Empty request = new();

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.GetAll),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _holidaysController.GetAll() as ObjectResult;
            HolidaysResponse actualData = actual.Value as HolidaysResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _holidaysClientMock.Verify(m => m.GetAll(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;

            Empty request = new();

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.GetAll),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _holidaysController.GetAll() as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _holidaysClientMock.Verify(m => m.GetAll(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByRangeDate_should_return_responce_from_grpc_client()
        {
            // Arrange
            HolidaysResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data.Add(new HolidayData
            {
                Id = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Description = "test",
                HolidayDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            });
            BaseMock.Response = response;

            ByDateRangeRequest request = new()
            {
                From = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                To = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1))
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.GetByRangeDate),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _holidaysController.GetByRangeDate(request) as ObjectResult;
            HolidaysResponse actualData = actual.Value as HolidaysResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _holidaysClientMock.Verify(m => m.GetByDateRange(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByRangeDate_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            ByDateRangeRequest request = new()
            {
                From = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                To = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1))
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(HolidaysController),
                CallerMethodName = nameof(_holidaysController.GetByRangeDate),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _holidaysController.GetByRangeDate(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "Status code as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _holidaysClientMock.Verify(m => m.GetByDateRange(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
