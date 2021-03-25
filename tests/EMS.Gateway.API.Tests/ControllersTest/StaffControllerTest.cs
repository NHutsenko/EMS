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
    public class StaffControllerTest : BaseUnitTest<StaffController>
    {
        private StaffController _staffController;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new StaffController(null, null, null));
            _staffController = new StaffController(_staffsClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void Add_should_return_response_from_grpc_client()
        {
            // Arrange
            StaffData request = new()
            {
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
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
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _staffController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _staffsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            StaffData request = new()
            {
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _staffController.Add(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _staffsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void Update_should_return_response_from_grpc_client()
        {
            // Arrange
            StaffData request = new()
            {
                Id = 1,
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
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
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _staffController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _staffsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            StaffData request = new()
            {
                Id = 1,
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _staffController.Update(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _staffsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void Delete_should_return_response_from_grpc_client()
        {
            // Arrange
            StaffData request = new()
            {
                Id = 1,
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
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
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _staffController.Delete(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _staffsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Delete_should_handle_exception()
        {
            // Arrange
            StaffData request = new()
            {
                Id = 1,
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.Delete),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _staffController.Delete(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _staffsClientMock.Verify(m => m.DeleteAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_return_response_from_grpc_client()
        {
            // Arrange
            Empty request = new();

            StaffResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data.Add(new StaffData
            {
                Id = 1,
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
            });
            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.GetAll),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _staffController.GetAll() as ObjectResult;
            StaffResponse actualData = actual.Value as StaffResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _staffsClientMock.Verify(m => m.GetAll(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_handle_exception()
        {
            // Arrange
            Empty request = new();

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.GetAll),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _staffController.GetAll() as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _staffsClientMock.Verify(m => m.GetAll(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByPersonId_should_return_response_from_grpc_client()
        {
            // Arrange
            ByPersonIdRequest request = new ByPersonIdRequest
            {
                PersonId = 2
            };

            StaffResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data.Add(new StaffData
            {
                Id = 1,
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
            });
            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _staffController.GetByPersonId(request) as ObjectResult;
            StaffResponse actualData = actual.Value as StaffResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _staffsClientMock.Verify(m => m.GetByPersonId(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByPersonId_should_handle_exception()
        {
            // Arrange
            ByPersonIdRequest request = new ByPersonIdRequest
            {
                PersonId = 2
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _staffController.GetByPersonId(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _staffsClientMock.Verify(m => m.GetByPersonId(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByManagerId_should_return_response_from_grpc_client()
        {
            // Arrange
            ByPersonIdRequest request = new ByPersonIdRequest
            {
                PersonId = 1
            };

            StaffResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data.Add(new StaffData
            {
                Id = 1,
                ManagerId = 1,
                PersonId = 2,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                MotivationModificatorId = 0,
                PositionId = 1
            });
            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.GetByManagerId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _staffController.GetByManagerId(request) as ObjectResult;
            StaffResponse actualData = actual.Value as StaffResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _staffsClientMock.Verify(m => m.GetByManagerId(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByManagerId_should_handle_exception()
        {
            // Arrange
            ByPersonIdRequest request = new ByPersonIdRequest
            {
                PersonId = 1
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(StaffController),
                CallerMethodName = nameof(_staffController.GetByManagerId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _staffController.GetByManagerId(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _staffsClientMock.Verify(m => m.GetByManagerId(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
