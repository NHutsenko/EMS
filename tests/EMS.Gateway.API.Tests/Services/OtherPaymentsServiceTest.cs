using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Core.API.Models;
using EMS.Core.API.Services;
using EMS.Core.API.Tests.Mock;
using Google.Protobuf.WellKnownTypes;
using Moq;
using NUnit.Framework;

namespace EMS.Core.API.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class OtherPaymentsServiceTest: BaseUnitTest<OtherPaymentsService>
    {
        private OtherPayment _otherPayment1;
        private OtherPayment _otherPayment2;
        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new OtherPaymentsService(null, null, null));

            _otherPayment1 = new OtherPayment
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Value = 10,
                Comment = "test1",
                PersonId = 1
            };
            _otherPayment2 = new OtherPayment
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddDays(20),
                Value = 10,
                Comment = "test2",
                PersonId = 1
            };
            _dbContext.OtherPayments.Add(_otherPayment1);
            _dbContext.OtherPayments.Add(_otherPayment2);
            _otherPaymentsRepository = new DAL.Repositories.OtherPaymentsRepository(_dbContext, _dateTimeUtil);
            _otherPaymentsService = new OtherPaymentsService(_otherPaymentsRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetByPersonId_should_return_all_other_payments_by_specified_person()
        {
            // Arrange
            ByPersonIdRequest request = new ByPersonIdRequest
            {
                PersonId = 1
            };

            OtherPaymentsResponse expectedResponse = new OtherPaymentsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            expectedResponse.Data.Add(new OtherPaymentData
            {
                Id = _otherPayment1.Id,
                Comment = _otherPayment1.Comment,
                CreatedOn = Timestamp.FromDateTime(_otherPayment1.CreatedOn),
                PersonId = _otherPayment1.PersonId,
                Value = _otherPayment1.Value
            });
            expectedResponse.Data.Add(new OtherPaymentData
            {
                Id = _otherPayment2.Id,
                Comment = _otherPayment2.Comment,
                CreatedOn = Timestamp.FromDateTime(_otherPayment2.CreatedOn),
                PersonId = _otherPayment2.PersonId,
                Value = _otherPayment2.Value
            });

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            OtherPaymentsResponse actual = _otherPaymentsService.GetByPersonId(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByPersonIdAndDateRange_should_return_all_other_payments_by_specified_person_and_date_range()
        {
            // Arrange
            ByPersonIdAndDateRangeRequest request = new ByPersonIdAndDateRangeRequest
            {
                Person = new ByPersonIdRequest
                {
                    PersonId = 1
                },
                Range = new ByDateRangeRequestRequest
                {
                    From = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                    To = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddDays(5))
                }
            };

            OtherPaymentsResponse expectedResponse = new OtherPaymentsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            expectedResponse.Data.Add(new OtherPaymentData
            {
                Id = _otherPayment1.Id,
                Comment = _otherPayment1.Comment,
                CreatedOn = Timestamp.FromDateTime(_otherPayment1.CreatedOn),
                PersonId = _otherPayment1.PersonId,
                Value = _otherPayment1.Value
            });

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.GetByPersonIdAndDateRange),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            OtherPaymentsResponse actual = _otherPaymentsService.GetByPersonIdAndDateRange(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_add_other_payment_to_db()
        {
            // Arrange
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(new DateTime(2021, 01, 16, 12, 0, 0, DateTimeKind.Utc)),
                Comment = "test",
                PersonId = 1,
                Value = 100
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                DataId = 3,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _otherPaymentsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                DataId = 0,
                ErrorMessage = "Other payment data cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.AddAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(new DateTime(2021, 01, 16, 12, 0, 0, DateTimeKind.Utc)),
                Comment = "test",
                PersonId = 1,
                Value = 0
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                DataId = 0,
                ErrorMessage = "Summ of payment cannot be equal to zero"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(new DateTime(2021, 01, 16, 12, 0, 0, DateTimeKind.Utc)),
                Comment = "test",
                PersonId = 1,
                Value = 10
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                DataId = 0,
                ErrorMessage = "An error occured while saving other payment"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _otherPaymentsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(new DateTime(2021, 01, 16, 12, 0, 0, DateTimeKind.Utc)),
                Comment = "test",
                PersonId = 1,
                Value = 10
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                DataId = 0,
                ErrorMessage = "Other payment has not been saved"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_update_other_payment_into_db()
        {
            // Arrange
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(new DateTime(2021, 01, 16, 12, 0, 0, DateTimeKind.Utc)),
                Comment = "test",
                PersonId = 1,
                Value = 100,
                Id = _otherPayment1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _otherPaymentsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                DataId = 0,
                ErrorMessage = "Other payment data cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.UpdateAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(new DateTime(2021, 01, 16, 12, 0, 0, DateTimeKind.Utc)),
                Comment = "test",
                PersonId = 1,
                Value = 0,
                Id = _otherPayment1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                DataId = 0,
                ErrorMessage = "Summ of payment cannot be equal to zero"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(new DateTime(2021, 01, 16, 12, 0, 0, DateTimeKind.Utc)),
                Comment = "test",
                PersonId = 1,
                Value = 10,
                Id = _otherPayment1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                DataId = 0,
                ErrorMessage = "An error occured while updating other payment"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _otherPaymentsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(new DateTime(2021, 01, 16, 12, 0, 0, DateTimeKind.Utc)),
                Comment = "test",
                PersonId = 1,
                Value = 10,
                Id = _otherPayment1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                DataId = 0,
                ErrorMessage = "Other payment has not been updated"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_delete_other_payment_from_db()
        {
            _otherPayment1.CreatedOn = _otherPayment1.CreatedOn.AddMonths(4);
            _dbContext.OtherPayments.Update(_otherPayment1);
            // Arrange
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(_otherPayment1.CreatedOn),
                Comment = _otherPayment1.Comment,
                PersonId = _otherPayment1.PersonId,
                Value = _otherPayment1.Value,
                Id = _otherPayment1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                DataId = _otherPayment1.Id,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _otherPaymentsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Other payment cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.DeleteAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_invalid_operation_exception()
        {
            // Arrange
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(_otherPayment1.CreatedOn),
                Comment = _otherPayment1.Comment,
                PersonId = _otherPayment1.PersonId,
                Value = _otherPayment1.Value,
                Id = _otherPayment1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Cannot delete history record"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_db_update_exception()
        {
            // Arrange
            _otherPayment1.CreatedOn = _otherPayment1.CreatedOn.AddMonths(4);
            _dbContext.OtherPayments.Update(_otherPayment1);
            DbContextMock.ShouldThrowException = true;
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(_otherPayment1.CreatedOn),
                Comment = _otherPayment1.Comment,
                PersonId = _otherPayment1.PersonId,
                Value = _otherPayment1.Value,
                Id = _otherPayment1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while deleting other payment"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _otherPaymentsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_exception()
        {
            // Arrange
            _otherPayment1.CreatedOn = _otherPayment1.CreatedOn.AddMonths(4);
            _dbContext.OtherPayments.Update(_otherPayment1);
            DbContextMock.SaveChangesResult = 0;
            OtherPaymentData request = new OtherPaymentData
            {
                CreatedOn = Timestamp.FromDateTime(_otherPayment1.CreatedOn),
                Comment = _otherPayment1.Comment,
                PersonId = _otherPayment1.PersonId,
                Value = _otherPayment1.Value,
                Id = _otherPayment1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Other payment has not been deleted"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(OtherPaymentsService),
                CallerMethodName = nameof(_otherPaymentsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _otherPaymentsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
