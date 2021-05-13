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
    public class OtherPaymentsControllerTests: BaseUnitTest<OtherPaymentsController>
    {
        private OtherPaymentsController _otherPaymentsController;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new OtherPaymentsController(null, null, null));
            _otherPaymentsController = new OtherPaymentsController(_otherPaymentsClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void Add_should_return_response_from_grpc_client()
        {
            // Arrange
            OtherPaymentData request = new()
            {
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Comment = "test",
                Value = 10
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
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _otherPaymentsController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            OtherPaymentData request = new()
            {
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Comment = "test",
                Value = 10
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _otherPaymentsController.Add(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_return_response_from_grpc_client()
        {
            // Arrange
            OtherPaymentData request = new()
            {
                Id = 1,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Comment = "test",
                Value = 10
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
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _otherPaymentsController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            OtherPaymentData request = new()
            {
                Id = 1,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Comment = "test",
                Value = 10
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _otherPaymentsController.Update(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_return_response_from_grpc_client()
        {
            // Arrange
            OtherPaymentData request = new()
            {
                Id = 1,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Comment = "test",
                Value = 10
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
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _otherPaymentsController.Delete(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Delete_should_handle_exception()
        {
            // Arrange
            OtherPaymentData request = new()
            {
                Id = 1,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Comment = "test",
                Value = 10
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _otherPaymentsController.Delete(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetByPersonId_should_return_response_from_grpc_client()
        {
            // Arrange
            ByPersonIdRequest request = new()
            {
                PersonId = 1
            };

            OtherPaymentsResponse response = new OtherPaymentsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data.Add(new OtherPaymentData
            {
                Id = 1,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Comment = "test",
                Value = 10
            });

            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _otherPaymentsController.GetByPersonId(request) as ObjectResult;
            OtherPaymentsResponse actualData = actual.Value as OtherPaymentsResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.GetByPersonId(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetByPersonId_should_handle_exception()
        {
            // Arrange
            ByPersonIdRequest request = new()
            {
                PersonId = 1
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _otherPaymentsController.GetByPersonId(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.GetByPersonId(request, null, null, new CancellationToken()), Times.Once);
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
                    To = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddDays(15))
                }
            };

            OtherPaymentsResponse response = new OtherPaymentsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data.Add(new OtherPaymentData
            {
                Id = 1,
                PersonId = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                Comment = "test",
                Value = 10
            });

            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.GetByPersonIdAndDateRange),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _otherPaymentsController.GetByPersonIdAndDateRange(request) as ObjectResult;
            OtherPaymentsResponse actualData = actual.Value as OtherPaymentsResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.GetByPersonIdAndDateRange(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetByPersonIdAndDateRange_should_handle_exception()
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
                    To = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddDays(15))
                }
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(OtherPaymentsController),
                CallerMethodName = nameof(_otherPaymentsController.GetByPersonIdAndDateRange),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _otherPaymentsController.GetByPersonIdAndDateRange(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _otherPaymentsClientMock.Verify(m => m.GetByPersonIdAndDateRange(request, null, null, new CancellationToken()), Times.Once);
        }
    }
}
