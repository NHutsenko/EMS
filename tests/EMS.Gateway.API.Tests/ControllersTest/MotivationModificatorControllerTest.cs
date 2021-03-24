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
    public class MotivationModificatorControllerTest: BaseUnitTest<MotivationModificatorController>
    {
        private MotivationModificatorController _motivationModificatorController;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new MotivationModificatorController(null, null, null));
            _motivationModificatorController = new MotivationModificatorController(_motivationModificatorsClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void Add_should_return_response_from_grpc_client()
        {
            // Arrange
            MotivationModificatorData request = new()
            {
                ModValue = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                StaffId = 1
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
                CallSide = nameof(MotivationModificatorController),
                CallerMethodName = nameof(_motivationModificatorController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _motivationModificatorController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _motivationModificatorsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            MotivationModificatorData request = new()
            {
                ModValue = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                StaffId = 1
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(MotivationModificatorController),
                CallerMethodName = nameof(_motivationModificatorController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _motivationModificatorController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _motivationModificatorsClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_return_response_from_grpc_client()
        {
            // Arrange
            MotivationModificatorData request = new()
            {
                Id = 1,
                ModValue = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                StaffId = 1
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
                CallSide = nameof(MotivationModificatorController),
                CallerMethodName = nameof(_motivationModificatorController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _motivationModificatorController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _motivationModificatorsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            MotivationModificatorData request = new()
            {
                Id = 1,
                ModValue = 1,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                StaffId = 1
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(MotivationModificatorController),
                CallerMethodName = nameof(_motivationModificatorController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _motivationModificatorController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _motivationModificatorsClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetByStaffId_should_return_response_from_grpc_client()
        {
            // Arrange
            ByStaffIdRequest request = new()
            {
                StaffId = 1
            };

            MotivationModificatorResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                },
                Data = new MotivationModificatorData
                {
                    Id = 1,
                    ModValue = 1,
                    CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                    StaffId = 1
                }
            };
            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(MotivationModificatorController),
                CallerMethodName = nameof(_motivationModificatorController.GetBystaffId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _motivationModificatorController.GetBystaffId(request.StaffId) as ObjectResult;
            MotivationModificatorResponse actualData = actual.Value as MotivationModificatorResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _motivationModificatorsClientMock.Verify(m => m.GetByStaffId(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetBystaffId_should_handle_exception()
        {
            // Arrange
            ByStaffIdRequest request = new ByStaffIdRequest
            {
                StaffId = 1
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(MotivationModificatorController),
                CallerMethodName = nameof(_motivationModificatorController.GetBystaffId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _motivationModificatorController.GetBystaffId(request.StaffId) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _motivationModificatorsClientMock.Verify(m => m.GetByStaffId(request, null, null, new CancellationToken()), Times.Once);
        }
    }
}
