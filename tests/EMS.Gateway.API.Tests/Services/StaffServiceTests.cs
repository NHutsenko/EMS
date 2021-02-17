using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
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
    public class StaffServiceTests: BaseUnitTest<StaffService>
    {
        private Staff _staff1;
        private Staff _staff2;
        private Position _position1;
        private Position _position2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new StaffService(null, null, null));

            _position1 = new Position
            {
                Id = 1
            };
            _position2 = new Position
            {
                Id = 2
            };
            _dbContext.Positions.Add(_position1);
            _dbContext.Positions.Add(_position2);

            _staff1 = new Staff
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime().AddDays(10),
                PositionId = _position1.Id,
                PersonId = 1,
                ManagerId = 2
            };

            _staff2 = new Staff
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PositionId = _position2.Id,
                PersonId = 2,
                ManagerId = 3
            };
            _dbContext.Staff.Add(_staff1);
            _dbContext.Staff.Add(_staff2);

            _staffRepository = new DAL.Repositories.StaffRepository(_dbContext, _dateTimeUtil);
            _staffService = new StaffService(_staffRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetAll_should_return_all_entities_from_db()
        {
            // Arrange
            StaffResponse expectedResponse = new StaffResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            Empty request = new Empty();

            expectedResponse.Data.Add(new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_staff1.CreatedOn),
                ManagerId = _staff1.ManagerId,
                MotivationModificatorId = _staff1.MotivationModificatorId.GetValueOrDefault(),
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff1.PositionId
            });
            expectedResponse.Data.Add(new StaffData
            {
                Id = _staff2.Id,
                CreatedOn = Timestamp.FromDateTime(_staff2.CreatedOn),
                ManagerId = _staff2.ManagerId,
                MotivationModificatorId = _staff2.MotivationModificatorId.GetValueOrDefault(),
                PersonId = _staff2.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId
            });

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.GetAll),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            StaffResponse actual = _staffService.GetAll(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByPersonId_should_return_all_entities_from_db_by_specified_person()
        {
            // Arrange
            StaffResponse expectedResponse = new StaffResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            ByPersonIdRequest request = new ByPersonIdRequest
            {
                PersonId = _staff1.PersonId.GetValueOrDefault()
            };

            expectedResponse.Data.Add(new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_staff1.CreatedOn),
                ManagerId = _staff1.ManagerId,
                MotivationModificatorId = _staff1.MotivationModificatorId.GetValueOrDefault(),
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff1.PositionId
            });

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            StaffResponse actual = _staffService.GetByPersonId(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetByPmanagerId_should_return_all_entities_from_db_by_specified_manager()
        {
            // Arrange
            StaffResponse expectedResponse = new StaffResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            ByPersonIdRequest request = new ByPersonIdRequest
            {
                PersonId = _staff2.ManagerId
            };

            expectedResponse.Data.Add(new StaffData
            {
                Id = _staff2.Id,
                CreatedOn = Timestamp.FromDateTime(_staff2.CreatedOn),
                ManagerId = _staff2.ManagerId,
                MotivationModificatorId = _staff2.MotivationModificatorId.GetValueOrDefault(),
                PersonId = _staff2.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId
            });

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.GetByManagerId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            StaffResponse actual = _staffService.GetByManagerId(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_add_new_staff_to_db()
        {
            // Arrange
            StaffData request = new StaffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1)),
                ManagerId = _staff1.ManagerId,
                PersonId = 0,
                PositionId = _staff2.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                DataId = 3,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _staffService.AddAsync(request, null).Result;

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
                ErrorMessage = "Staff entity cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.AddAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            StaffData request = new StaffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(-1)),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Cannot add new staff record into existing work period"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            StaffData request = new StaffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1)),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving staff data"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _staffService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            StaffData request = new StaffData
            {
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1)),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Staff has not been saved"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_add_new_staff_to_db()
        {
            // Arrange
            StaffData request = new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1)),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId,
                MotivationModificatorId = 1
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                DataId = _staff1.Id,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _staffService.UpdateAsync(request, null).Result;

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
                ErrorMessage = "Staff entity cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.UpdateAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            Staff staff = new Staff
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = _staff1.PersonId,
                ManagerId = _staff1.ManagerId,
                Id = 3
            };
            _dbContext.Staff.Add(staff);
            StaffData request = new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(-1)),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Cannot update existing staff record into existing work period"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            StaffData request = new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1)),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while updating staff"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _staffService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            StaffData request = new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().AddMonths(1)),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff2.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Staff has not been updated"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_delete_record_from_db()
        {
            // Arrange
            _staff1.CreatedOn = _staff1.CreatedOn.AddDays(10);
            _dbContext.Staff.Update(_staff1);

            StaffData request = new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_staff1.CreatedOn),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff1.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.Success,
                DataId = _staff1.Id,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = expectedResponse
            };

            // Act
            BaseResponse actual = _staffService.DeleteAsync(request, null).Result;

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
                ErrorMessage = "Staff data cannot be empty"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = null,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.DeleteAsync(null, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_invalid_operation_exception()
        {
            // Arrange
            _staff1.CreatedOn = _staff1.CreatedOn.AddDays(-15);
            _dbContext.Staff.Update(_staff1);

            StaffData request = new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_staff1.CreatedOn),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff1.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DataError,
                ErrorMessage = "Cannot delete history record"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            _staff1.CreatedOn = _staff1.CreatedOn.AddDays(10);
            _dbContext.Staff.Update(_staff1);

            StaffData request = new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_staff1.CreatedOn),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff1.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while deleting staff"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("DbContext test Exception")
            };

            // Act
            BaseResponse actual = _staffService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            _staff1.CreatedOn = _staff1.CreatedOn.AddDays(10);
            _dbContext.Staff.Update(_staff1);

            StaffData request = new StaffData
            {
                Id = _staff1.Id,
                CreatedOn = Timestamp.FromDateTime(_staff1.CreatedOn),
                ManagerId = _staff1.ManagerId,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                PositionId = _staff1.PositionId
            };

            BaseResponse expectedResponse = new BaseResponse
            {
                Code = Code.UnknownError,
                ErrorMessage = "Staff has not been deleted"
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(StaffService),
                CallerMethodName = nameof(_staffService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(expectedResponse.ErrorMessage)
            };

            // Act
            BaseResponse actual = _staffService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(expectedResponse, actual, "Response as expected");
            _loggerMock.Verify(mocks => mocks.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
