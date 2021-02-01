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
    public class DayOffsServiceTest: BaseUnitTest<DayOffsService>
    {
        private DayOff _dayOff1;
        private DayOff _dayOff2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new DayOffsService(null, null, null));
            DbContextMock.ShouldThrowException = false;
            DbContextMock.SaveChangesResult = 1;

            _dayOff1 = new DayOff
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                DayOffType = Enums.DayOffType.Vacation,
                Hours = 8,
                IsPaid = true,
                PersonId = 1
            };
            _dayOff2 = new DayOff
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddMonths(1),
                DayOffType = Enums.DayOffType.Vacation,
                Hours = 8,
                IsPaid = true,
                PersonId = 1
            };

            _dbContext.DaysOff.Add(_dayOff1);
            _dbContext.DaysOff.Add(_dayOff2);

            _dayOffRepository = new DAL.Repositories.DayOffRepository(_dbContext, _dateTimeUtil);
            _dayOffsService = new DayOffsService(_dayOffRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetByPersionId_should_return_all_day_offs_by_specified_person()
        {
            // Arrange
            ByPersonIdRequest request = new ByPersonIdRequest
            {
                PersonId = _dayOff1.PersonId
            };

            DayOffsResponse expectedResponse = new DayOffsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            expectedResponse.Data.Add(new DayOffData
            {
                Id = _dayOff1.Id,
                CreatedOn = Timestamp.FromDateTime(_dayOff1.CreatedOn),
                DayOffType = (int)_dayOff1.DayOffType,
                Hours = _dayOff1.Hours,
                IsPaid = _dayOff1.IsPaid,
                PersonId = _dayOff1.PersonId
            });
            expectedResponse.Data.Add(new DayOffData
            {
                Id = _dayOff2.Id,
                CreatedOn = Timestamp.FromDateTime(_dayOff2.CreatedOn),
                DayOffType = (int)_dayOff2.DayOffType,
                Hours = _dayOff2.Hours,
                IsPaid = _dayOff2.IsPaid,
                PersonId = _dayOff2.PersonId
            });

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            DayOffsResponse actual = _dayOffsService.GetByPersonId(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByPersionIdAndDateRange_should_return_all_day_offs_by_specified_person()
        {
            // Arrange
            ByPersonIdDateRangeRequestRequest request = new ByPersonIdDateRangeRequestRequest
            {
                Person = new ByPersonIdRequest { PersonId = 1 },
                Range = new ByDateRangeRequestRequest
                {
                    From = Timestamp.FromDateTime(new DateTime(2020, 12, 15, 0, 0,0, DateTimeKind.Utc)),
                    To = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc))
                }
            };

            DayOffsResponse expectedResponse = new DayOffsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            expectedResponse.Data.Add(new DayOffData
            {
                Id = _dayOff1.Id,
                CreatedOn = Timestamp.FromDateTime(_dayOff1.CreatedOn),
                DayOffType = (int)_dayOff1.DayOffType,
                Hours = _dayOff1.Hours,
                IsPaid = _dayOff1.IsPaid,
                PersonId = _dayOff1.PersonId
            });

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.GetByPersonIdAndDateRange),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            DayOffsResponse actual = _dayOffsService.GetByPersonIdAndDateRange(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_add_day_off_to_db()
        {
            // Arrange
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddDays(5)),
                DayOffType = 1,
                Hours = 5,
                IsPaid = true,
                PersonId = 1
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = 3
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _dayOffsService.AddAsync(request, null).Result;

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
                ErrorMessage = "Day off cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.AddAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1,
                Hours = 5,
                IsPaid = true
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Cannot add day off record without specified person"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_DbUpdate_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddDays(5)),
                DayOffType = 1,
                Hours = 5,
                IsPaid = true,
                PersonId = 1
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving day off"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _dayOffsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1,
                Hours = 5,
                IsPaid = true,
                PersonId = 1
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Day off has not been saved"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_update_day_off_into_db()
        {
            // Arrange
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1,
                Hours = 5,
                IsPaid = true,
                PersonId = 1,
                Id = _dayOff1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = 1
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _dayOffsService.UpdateAsync(request, null).Result;

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
                ErrorMessage = "Day off cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.UpdateAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1,
                Hours = 5,
                IsPaid = true,
                Id = _dayOff1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Cannot add day off record without specified person"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_DbUpdate_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1,
                Hours = 5,
                IsPaid = true,
                PersonId = 1,
                Id = _dayOff1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while updating day off"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _dayOffsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                DayOffType = 1,
                Hours = 5,
                IsPaid = true,
                PersonId = 1,
                Id = _dayOff1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Day off has not been saved"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_delete_record_from_db()
        {
            // Arrange
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dayOff1.CreatedOn),
                DayOffType = (int)_dayOff1.DayOffType,
                Hours = _dayOff1.Hours,
                IsPaid = _dayOff1.IsPaid,
                PersonId = _dayOff1.PersonId,
                Id = _dayOff1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty,
                DataId = _dayOff1.Id
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _dayOffsService.DeleteAsync(request, null).Result;

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
                ErrorMessage = "Day off cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.DeleteAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_invalid_operation_exception()
        {
            // Arrange
            _dayOff1.CreatedOn = _dayOff1.CreatedOn.AddMonths(-5);
            _dbContext.DaysOff.Update(_dayOff1);
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dayOff1.CreatedOn),
                DayOffType = (int)_dayOff1.DayOffType,
                Hours = _dayOff1.Hours,
                IsPaid = _dayOff1.IsPaid,
                PersonId = _dayOff1.PersonId,
                Id = _dayOff1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Cannot delete history record"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_DbUpdate_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dayOff1.CreatedOn),
                DayOffType = (int)_dayOff1.DayOffType,
                Hours = _dayOff1.Hours,
                IsPaid = _dayOff1.IsPaid,
                PersonId = _dayOff1.PersonId,
                Id = _dayOff1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while deleting day off"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _dayOffsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            DayOffData request = new DayOffData
            {
                CreatedOn = Timestamp.FromDateTime(_dayOff1.CreatedOn),
                DayOffType = (int)_dayOff1.DayOffType,
                Hours = _dayOff1.Hours,
                IsPaid = _dayOff1.IsPaid,
                PersonId = _dayOff1.PersonId,
                Id = _dayOff1.Id
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Day off has not been deleted"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(_dayOffsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _dayOffsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
