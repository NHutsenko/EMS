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
    public class DayOffsControllerTests : BaseUnitTest<DayOffsController>
    {
        private DayOffsController _dayOffsController;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new DayOffsController(null, null, null));
            _dayOffsController = new DayOffsController(_dayOffsClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void Add_should_return_response_from_grpc_client()
        {
            // Arrange
            DayOffData request = new()
            {
                IsPaid = true,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1
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
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _dayOffsController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _dayOffsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            DayOffData request = new()
            {
                IsPaid = true,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _dayOffsController.Add(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response data as expected");
            _dayOffsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void Update_should_return_response_from_grpc_client()
        {
            // Arrange
            DayOffData request = new()
            {
                Id = 1,
                IsPaid = true,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1
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
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _dayOffsController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _dayOffsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            DayOffData request = new()
            {
                IsPaid = true,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _dayOffsController.Update(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response data as expected");
            _dayOffsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void Delete_should_return_response_from_grpc_client()
        {
            // Arrange
            DayOffData request = new()
            {
                Id = 1,
                IsPaid = true,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1
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
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _dayOffsController.Delete(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _dayOffsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Delete_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            DayOffData request = new()
            {
                IsPaid = true,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _dayOffsController.Delete(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response data as expected");
            _dayOffsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByPersonId_should_return_response_from_grpc_client()
        {
            // Arrange
            ByPersonIdRequest request = new()
            {
                PersonId = 1
            };

            DayOffsResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            BaseMock.Response = response;
            response.Data.Add(new DayOffData
            {
                IsPaid = true,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1
            });

            LogData expectedLog = new()
            {
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _dayOffsController.GetByPersonId(request) as ObjectResult;
            DayOffsResponse actualData = actual.Value as DayOffsResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _dayOffsClientMock.Verify(m => m.GetByPersonId(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetByPersonId_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            ByPersonIdRequest request = new()
            {
                PersonId = 1
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _dayOffsController.GetByPersonId(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _dayOffsClientMock.Verify(m => m.GetByPersonId(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetByPersonIdAndDateRange_should_return_response_from_grpc_client()
        {
            // Arrange
            ByPersonIdAndDateRangeRequest request = new()
            {
                Person = new ByPersonIdRequest
                {
                    PersonId = 1
                },
                Range = new ByDateRangeRequest
                {
                    From = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                    To = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddDays(10))
                }
            };

            DayOffsResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            response.Data.Add(new DayOffData
            {
                IsPaid = true,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1
            });
            BaseMock.Response = response;
            LogData expectedLog = new()
            {
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.GetByPersonIdAndDateRange),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _dayOffsController.GetByPersonIdAndDateRange(request) as ObjectResult;
            DayOffsResponse actualData = actual.Value as DayOffsResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _dayOffsClientMock.Verify(m => m.GetByPersonIdAndDateRange(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetByPersonIdAndDateRange_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            ByPersonIdAndDateRangeRequest request = new()
            {
                Person = new ByPersonIdRequest
                {
                    PersonId = 1
                },
                Range = new ByDateRangeRequest
                {
                    From = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                    To = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddDays(10))
                }
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(DayOffsController),
                CallerMethodName = nameof(_dayOffsController.GetByPersonIdAndDateRange),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _dayOffsController.GetByPersonIdAndDateRange(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual("An error occured while sending request", actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _dayOffsClientMock.Verify(m => m.GetByPersonIdAndDateRange(request, null, null, new CancellationToken()), Times.Once);
        }
    }
}
