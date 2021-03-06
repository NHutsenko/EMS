﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class MotivationModificatorsServiceTests : BaseUnitTest<MotivationModificatorsService>
    {
        private MotivationModificator _motivationModificator1;
        private Staff _staff1;
        private Staff _staff2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new MotivationModificatorsService(null, null, null));

            _staff1 = new Staff
            {
                Id = 1
            };

            _staff2 = new Staff
            {
                Id = 2
            };
            _dbContext.Staff.Add(_staff1);
            _dbContext.Staff.Add(_staff2);

            _motivationModificator1 = new MotivationModificator
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ModValue = 0.89,
                StaffId = _staff1.Id
            };

            _dbContext.MotivationModificators.Add(_motivationModificator1);

            _motivationModificatorsService = new MotivationModificatorsService(_motivationModificatorRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void AddAsync_should_save_motivation_modificator()
        {
            // Arrange
            MotivationModificatorData request = new()
			{
				StaffId = _staff2.Id,
				ModValue = 0.3
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				DataId = 2,
				ErrorMessage = string.Empty
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_null_reference_exception()
        {
            // Arrange
            MotivationModificatorData request = null;

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Motificator cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            MotivationModificatorData request = new()
			{
				StaffId = 4,
				ModValue = 0.5
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Cannot modify motivation for work period which does not exists"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_invalid_operation_exception()
        {
            // Arrange
            MotivationModificatorData request = new()
			{
				StaffId = _staff1.Id,
				ModValue = 0.5
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Motivation modificator already exists for specified work period"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            MotivationModificatorData request = new()
			{
				StaffId = _staff2.Id,
				ModValue = 0.5
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while saving motivation modificator"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(DbContextMock.ExceptionMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            MotivationModificatorData request = new()
			{
				StaffId = _staff2.Id,
				ModValue = 0.5
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Motivation modificator has not been saved"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_save_motivation_modificator()
        {
            // Arrange
            MotivationModificatorData request = new()
			{
				Id = _motivationModificator1.Id,
				StaffId = _motivationModificator1.StaffId,
				ModValue = 0.3,
				CreatedOn = Timestamp.FromDateTime(_motivationModificator1.CreatedOn)
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				DataId = _motivationModificator1.Id,
				ErrorMessage = string.Empty
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_null_reference_exception()
        {
            // Arrange
            MotivationModificatorData request = null;

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Motificator cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            MotivationModificatorData request = new()
			{
				Id = _motivationModificator1.Id,
				StaffId = _motivationModificator1.StaffId,
				ModValue = -1,
				CreatedOn = Timestamp.FromDateTime(_motivationModificator1.CreatedOn)
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Motivation mod cannot be less than 0 or equal to 0"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            MotivationModificatorData request = new()
			{
				Id = _motivationModificator1.Id,
				StaffId = _motivationModificator1.StaffId,
				ModValue = 0.5
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while updating motivation modificator"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(DbContextMock.ExceptionMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            MotivationModificatorData request = new()
			{
				Id = _motivationModificator1.Id,
				StaffId = _motivationModificator1.StaffId,
				ModValue = 0.5
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Motivation modificator has not been updated"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _motivationModificatorsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByStaffId_should_return_modificator_by_staff_id()
        {
            // Arrange
            MotivationModificatorResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty },
				Data = new MotivationModificatorData { Id = _motivationModificator1.Id, ModValue = _motivationModificator1.ModValue, StaffId = _motivationModificator1.StaffId, CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()) }
			};

            ByStaffIdRequest request = new()
			{
				StaffId = _motivationModificator1.StaffId
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.GetByStaffId),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            MotivationModificatorResponse actual = _motivationModificatorsService.GetByStaffId(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByStaffId_should_return_empty_modificator()
        {
            // Arrange
            MotivationModificatorResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty },
				Data = null
			};

            ByStaffIdRequest request = new()
			{
				StaffId = _staff2.Id
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.GetByStaffId),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            MotivationModificatorResponse actual = _motivationModificatorsService.GetByStaffId(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByStaffId_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            MotivationModificatorResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.UnknownError, ErrorMessage = "An error occured while loading motivation modificator data" },
				Data = null
			};

            ByStaffIdRequest request = new()
			{
				StaffId = _staff2.Id
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(MotivationModificatorsService),
				CallerMethodName = nameof(_motivationModificatorsService.GetByStaffId),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("Test exception")
			};

            // Act
            MotivationModificatorResponse actual = _motivationModificatorsService.GetByStaffId(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
