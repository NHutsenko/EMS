using System;
using System.Diagnostics.CodeAnalysis;
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
    public class HolidaysServiceTest: BaseUnitTest<HolidaysService>
    {
        public Holiday _holiday1;
        public Holiday _holiday2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new HolidaysService(null, null, null));

            _holiday1 = new Holiday
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Description = "Test holiday one",
                HolidayDate = new DateTime(2021, 2, 1, 12, 0, 0, DateTimeKind.Utc),
                ToDoDate = new DateTime(2021, 1, 3, 12, 0, 0, DateTimeKind.Utc)
            };
            _holiday2 = new Holiday
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Description = "Test holiday one",
                HolidayDate = new DateTime(2021, 1, 7, 12, 0, 0, DateTimeKind.Utc)
            };

            _dbContext.Holidays.Add(_holiday1);
            _dbContext.Holidays.Add(_holiday2);

            _holidaysService = new HolidaysService(_holidaysRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetAll_should_return_all_hoildays_from_db()
        {
            // Arrange
            Empty request = new Empty();

            HolidaysResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};

            expectedResponse.Data.Add(new HolidayData
            {
                Id = _holiday1.Id,
                CreatedOn = Timestamp.FromDateTime(_holiday1.CreatedOn),
                Description = _holiday1.Description,
                HolidayDate = Timestamp.FromDateTime(_holiday1.HolidayDate),
                ToDoDate = _holiday1.ToDoDate.HasValue ? Timestamp.FromDateTime(_holiday1.ToDoDate.Value): Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime()) 
            });
            expectedResponse.Data.Add(new HolidayData
            {
                Id = _holiday2.Id,
                CreatedOn = Timestamp.FromDateTime(_holiday2.CreatedOn),
                Description = _holiday2.Description,
                HolidayDate = Timestamp.FromDateTime(_holiday2.HolidayDate),
                ToDoDate = _holiday2.ToDoDate.HasValue ? Timestamp.FromDateTime(_holiday2.ToDoDate.Value) : Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            });

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            HolidaysResponse actual = _holidaysService.GetAll(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Returned holidays as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            Empty request = new Empty();

            HolidaysResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.UnknownError, ErrorMessage = "An error occured while loading holidays data" }
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.GetAll),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("Test exception")
			};

            // Act
            HolidaysResponse actual = _holidaysService.GetAll(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Handled exception as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByDateRange_should_return_hoildays_by_date_range_from_db()
        {
            // Arrange
            DateRangeRequest request = new()
			{
				From = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
				To = Timestamp.FromDateTime(new DateTime(2021, 1, 10, 0, 0, 0, DateTimeKind.Utc))
			};

            HolidaysResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};

            expectedResponse.Data.Add(new HolidayData
            {
                Id = _holiday2.Id,
                CreatedOn = Timestamp.FromDateTime(_holiday2.CreatedOn),
                Description = _holiday2.Description,
                HolidayDate = Timestamp.FromDateTime(_holiday2.HolidayDate),
                ToDoDate = _holiday2.ToDoDate.HasValue ? Timestamp.FromDateTime(_holiday2.ToDoDate.Value) : Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            });

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.GetByDateRange),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            HolidaysResponse actual = _holidaysService.GetByDateRange(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Returned holidays as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByDateRange_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            DateRangeRequest request = new()
			{
				From = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
				To = Timestamp.FromDateTime(new DateTime(2021, 1, 10, 0, 0, 0, DateTimeKind.Utc))
			};

            HolidaysResponse expectedResponse = new()
			{
				Status = new BaseResponse { Code = Code.UnknownError, ErrorMessage = "An error occured while loading holidays data" }
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.GetByDateRange),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("Test exception")
			};

            // Act
            HolidaysResponse actual = _holidaysService.GetByDateRange(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Handled exception as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_add_holiday_to_db_without_todo_date()
        {
            // Arrange
            HolidayData request = new()
			{
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime()),
				HolidayDate = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc)),
				ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = 3
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            BaseResponse actual = _holidaysService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Added as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_add_holiday_to_db_with_todo_date()
        {
            // Arrange
            HolidayData request = new()
			{
				Description = "test",
				HolidayDate = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc)),
				ToDoDate = Timestamp.FromDateTime(new DateTime(2021, 1, 16, 0, 0, 0, DateTimeKind.Utc))
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = 3
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            BaseResponse actual = _holidaysService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Holiday cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = null,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.AddAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            HolidayData request = new()
			{
				Description = "test",
				CreatedOn = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime()),
				HolidayDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime()),
				ToDoDate = Timestamp.FromDateTime(new DateTime(2021, 1, 16, 0, 0, 0, DateTimeKind.Utc))
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Holiday date cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            HolidayData request = new()
			{
				Description = "test",
				HolidayDate = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc)),
				ToDoDate = Timestamp.FromDateTime(new DateTime(2021, 1, 16, 0, 0, 0, DateTimeKind.Utc))
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while saving holiday"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("DbContext test Exception")
			};

            // Act
            BaseResponse actual = _holidaysService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            HolidayData request = new()
			{
				Description = "test",
				HolidayDate = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc)),
				ToDoDate = Timestamp.FromDateTime(new DateTime(2021, 1, 16, 0, 0, 0, DateTimeKind.Utc))
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Holiday has not been saved"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.AddAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_add_holiday_to_db_without_todo_date()
        {
            // Arrange
            HolidayData request = new()
			{
				Id = _holiday1.Id,
				CreatedOn = Timestamp.FromDateTime(_holiday1.CreatedOn),
				Description = "test",
				HolidayDate = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc)),
				ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = request.Id
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            BaseResponse actual = _holidaysService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Added as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_add_holiday_to_db_with_todo_date()
        {
            // Arrange
            HolidayData request = new()
			{
				Id = _holiday1.Id,
				Description = "test",
				HolidayDate = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc)),
				ToDoDate = Timestamp.FromDateTime(new DateTime(2021, 1, 16, 0, 0, 0, DateTimeKind.Utc))
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = request.Id
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            BaseResponse actual = _holidaysService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Holiday cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = null,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.UpdateAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            HolidayData request = new()
			{
				Id = _holiday1.Id,
				CreatedOn = Timestamp.FromDateTime(_holiday1.CreatedOn),
				Description = "test",
				HolidayDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime()),
				ToDoDate = Timestamp.FromDateTime(new DateTime(2021, 1, 16, 0, 0, 0, DateTimeKind.Utc))
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Holiday date cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            HolidayData request = new()
			{
				Id = _holiday1.Id,
				Description = "test",
				HolidayDate = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc)),
				ToDoDate = Timestamp.FromDateTime(new DateTime(2021, 1, 16, 0, 0, 0, DateTimeKind.Utc))
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while updating holiday"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("DbContext test Exception")
			};

            // Act
            BaseResponse actual = _holidaysService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            HolidayData request = new()
			{
				Id = _holiday1.Id,
				Description = "test",
				HolidayDate = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc)),
				ToDoDate = Timestamp.FromDateTime(new DateTime(2021, 1, 16, 0, 0, 0, DateTimeKind.Utc))
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Holiday has not been updated"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.UpdateAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_delete_holiday_from_db()
        {
            // Arrange
            HolidayData request = new()
			{
				Id = _holiday1.Id,
				CreatedOn = Timestamp.FromDateTime(_holiday1.CreatedOn),
				Description = _holiday1.Description,
				HolidayDate = Timestamp.FromDateTime(_holiday1.HolidayDate),
				ToDoDate = Timestamp.FromDateTime(_holiday1.ToDoDate.Value)
			};

            BaseResponse expectedResponse = new()
			{
				Code = Code.Success,
				ErrorMessage = string.Empty,
				DataId = request.Id
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = expectedResponse
			};

            // Act
            BaseResponse actual = _holidaysService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_null_reference_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Holiday cannot be empty"
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = null,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.DeleteAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_invalid_operation_exception()
        {
            // Arrange
            BaseResponse expectedResponse = new()
			{
				Code = Code.DataError,
				ErrorMessage = "Cannot delete history record"
			};

            HolidayData request = new()
			{
				Id = _holiday2.Id,
				CreatedOn = Timestamp.FromDateTime(_holiday2.CreatedOn),
				Description = _holiday2.Description,
				HolidayDate = Timestamp.FromDateTime(_holiday2.HolidayDate),
				ToDoDate = Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            BaseResponse expectedResponse = new()
			{
				Code = Code.DbError,
				ErrorMessage = "An error occured while deleting holiday"
			};

            HolidayData request = new()
			{
				Id = _holiday1.Id,
				CreatedOn = Timestamp.FromDateTime(_holiday1.CreatedOn),
				Description = _holiday1.Description,
				HolidayDate = Timestamp.FromDateTime(_holiday1.HolidayDate),
				ToDoDate = Timestamp.FromDateTime(_holiday1.ToDoDate.GetValueOrDefault())
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception("DbContext test Exception")
			};

            // Act
            BaseResponse actual = _holidaysService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            BaseResponse expectedResponse = new()
			{
				Code = Code.UnknownError,
				ErrorMessage = "Holiday has not been deleted"
			};

            HolidayData request = new()
			{
				Id = _holiday1.Id,
				CreatedOn = Timestamp.FromDateTime(_holiday1.CreatedOn),
				Description = _holiday1.Description,
				HolidayDate = Timestamp.FromDateTime(_holiday1.HolidayDate),
				ToDoDate = Timestamp.FromDateTime(_holiday1.ToDoDate.GetValueOrDefault())
			};

            LogData expectedLog = new()
			{
				CallSide = nameof(HolidaysService),
				CallerMethodName = nameof(_holidaysService.DeleteAsync),
				CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
				Request = request,
				Response = new Exception(expectedResponse.ErrorMessage)
			};

            // Act
            BaseResponse actual = _holidaysService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }
    }
}

