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
    public class SalaryControllerTests: BaseUnitTest<SalaryController>
    {
        private SalaryController _salaryController;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new SalaryController(null, null, null));
            _salaryController = new SalaryController(_salaryClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetSalary_should_return_response_from_grpc_client()
        {
            // Arrange
            ISalaryResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.SalaryResponse.Add(new SalaryResponse
            {
                CurrentPosition = 1,
                ManagerId = 1,
                PersonId = 2,
                Salary = 100,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime())
            });
            BaseMock.Response = response;

            SalaryRequest request = new()
            {
                ManagerId = 1,
                StartDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                EndDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1))
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(SalaryController),
                CallerMethodName = nameof(_salaryController.GetSalary),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _salaryController.GetSalary(request) as ObjectResult;
            ISalaryResponse actualData = actual.Value as ISalaryResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _salaryClientMock.Verify(m => m.GetSalary(request, null, null, new CancellationToken()), Times.Once);
        }

        [Test]
        public void GetSalary_should__handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException  = true;

            SalaryRequest request = new()
            {
                ManagerId = 1,
                StartDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                EndDate = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1))
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(SalaryController),
                CallerMethodName = nameof(_salaryController.GetSalary),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _salaryController.GetSalary(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _salaryClientMock.Verify(m => m.GetSalary(request, null, null, new CancellationToken()), Times.Once);
        }
    }
}
