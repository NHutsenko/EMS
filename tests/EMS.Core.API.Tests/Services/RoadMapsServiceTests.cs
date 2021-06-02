using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Core.API.Enums;
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
    public class RoadMapsServiceTests: BaseUnitTest<RoadMapsService>
    {
        private RoadMap _roadMap;
        private Staff _staff;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new RoadMapsService(null, null, null));

            _staff = new Staff
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PersonId = 1,
                PositionId = 1,
            };
            _roadMap = new RoadMap
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                StaffId = _staff.Id,
                Staff= _staff,
                Status = Enums.RoadMapStatus.InProgress,
                Tasks = "test"
            };
            _staff.RoadMapId = _roadMap.Id;
            _staff.RoadMap = _roadMap;
            _dbContext.RoadMaps.Add(_roadMap);
            _dbContext.Staff.Add(_staff);

            _roadMapsService = new RoadMapsService(_roadMapRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetByStaffId_should_return_road_map_accociated_with_staff_id()
        {
            // Arrange
            ByStaffRequest request = new()
            {
                StaffId = _staff.Id
            };

            RoadMapResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                },
                Data = new RoadMapData()
                {
                    Id = _roadMap.Id,
                    StaffId = _roadMap.StaffId,
                    CreatedOn = Timestamp.FromDateTime(_roadMap.CreatedOn),
                    Status = (int)_roadMap.Status,
                    Tasks = _roadMap.Tasks
                }
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.GetByStaffId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            RoadMapResponse actual = _roadMapsService.GetByStaffId(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Rasponse as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.GetByStaffId(request.StaffId), Times.Once);
        }

        [Test]
        public void GetByStaffId_should_handle_exception()
        {
            // Arrange
            BaseMock.ShouldThrowException = true;
            ByStaffRequest request = new()
            {
                StaffId = _staff.Id
            };

            RoadMapResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = "An error occured while loading Road Map"
                }
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.GetByStaffId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            RoadMapResponse actual = _roadMapsService.GetByStaffId(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Rasponse as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.GetByStaffId(request.StaffId), Times.Once);
        }

        [Test]
        public void AddAsync_should_save_new_road_map_via_road_map_repository()
        {
            // Arrange
            _staff.RoadMap = null;
            _staff.RoadMapId = 0;
            RoadMapData request = new()
            {
                StaffId = _staff.Id,
                Status = 0,
                Tasks = "Test"
            };
            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 2,
                ErrorMessage = string.Empty
            };

            RoadMap expectedRoadMap = new()
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                StaffId = request.StaffId,
                Status = (RoadMapStatus)System.Enum.Parse(typeof(RoadMapStatus), request.Status.ToString(), true),
                Tasks = request.Tasks
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            BaseResponse actual = _roadMapsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.AddAsync(expectedRoadMap), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_argument_exception()
        {
            // Arrange
            RoadMapData request = new()
            {
                Status = 0,
                Tasks = "Test"
            };
            BaseResponse response = new()
            {
                Code = Code.DataError,
                ErrorMessage = "Cannot create road map for non-existent work period"
            };

            RoadMap expectedRoadMap = new()
            {
                Status = (RoadMapStatus)System.Enum.Parse(typeof(RoadMapStatus), request.Status.ToString(), true),
                Tasks = request.Tasks
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Cannot create road map for non-existent work period")
            };

            // Act
            BaseResponse actual = _roadMapsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.AddAsync(expectedRoadMap), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_invalid_operation_exception()
        {
            // Arrange
            RoadMapData request = new()
            {
                StaffId = _staff.Id,
                Status = 0,
                Tasks = "Test"
            };
            BaseResponse response = new()
            {
                Code = Code.DataError,
                ErrorMessage = "Road map already exists"
            };

            RoadMap expectedRoadMap = new()
            {
                StaffId = _staff.Id,
                Status = (RoadMapStatus)System.Enum.Parse(typeof(RoadMapStatus), request.Status.ToString(), true),
                Tasks = request.Tasks
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Road map already exists")
            };

            // Act
            BaseResponse actual = _roadMapsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.AddAsync(expectedRoadMap), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            _staff.RoadMap = null;
            _staff.RoadMapId = 0;
            RoadMapData request = new()
            {
                StaffId = _staff.Id,
                Status = 0,
                Tasks = "Test"
            };
            BaseResponse response = new()
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while saving road map"
            };

            RoadMap expectedRoadMap = new()
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                StaffId = _staff.Id,
                Status = (RoadMapStatus)System.Enum.Parse(typeof(RoadMapStatus), request.Status.ToString(), true),
                Tasks = request.Tasks
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(DbContextMock.ExceptionMessage)
            };

            // Act
            BaseResponse actual = _roadMapsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.AddAsync(expectedRoadMap), Times.Once);
        }

        [Test]
        public void AddAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            _staff.RoadMap = null;
            _staff.RoadMapId = 0;
            RoadMapData request = new()
            {
                StaffId = _staff.Id,
                Status = 0,
                Tasks = "Test"
            };
            BaseResponse response = new()
            {
                Code = Code.UnknownError,
                ErrorMessage = "Road map was not saved"
            };

            RoadMap expectedRoadMap = new()
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                StaffId = _staff.Id,
                Status = (RoadMapStatus)System.Enum.Parse(typeof(RoadMapStatus), request.Status.ToString(), true),
                Tasks = request.Tasks
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.AddAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Road map was not saved")
            };

            // Act
            BaseResponse actual = _roadMapsService.AddAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.AddAsync(expectedRoadMap), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_update_existing_road_map_via_road_map_repository()
        {
            // Arrange
            RoadMapData request = new()
            {
                Id = _roadMap.Id,
                CreatedOn = Timestamp.FromDateTime(_roadMap.CreatedOn),
                StaffId = _roadMap.StaffId,
                Status = (int)_roadMap.Status,
                Tasks = "Test update"
            };
            _roadMap.Tasks = request.Tasks;
            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            BaseResponse actual = _roadMapsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.UpdateAsync(_roadMap), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_argument_exception()
        {
            // Arrange
            RoadMapData request = new()
            {
                Id = _roadMap.Id,
                CreatedOn = Timestamp.FromDateTime(_roadMap.CreatedOn),
                StaffId = _roadMap.StaffId,
                Status = (int)_roadMap.Status,
                Tasks = string.Empty
            };
            _roadMap.Tasks = string.Empty;
            BaseResponse response = new()
            {
                Code = Code.DataError,
                ErrorMessage = "Tasks for road map was not provided"
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Tasks for road map was not provided")
            };

            // Act
            BaseResponse actual = _roadMapsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.UpdateAsync(_roadMap), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            RoadMapData request = new()
            {
                Id = _roadMap.Id,
                CreatedOn = Timestamp.FromDateTime(_roadMap.CreatedOn),
                StaffId = _roadMap.StaffId,
                Status = (int)_roadMap.Status,
                Tasks = "Test update"
            };
            _roadMap.Tasks = request.Tasks;
            BaseResponse response = new()
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while updating road map"
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(DbContextMock.ExceptionMessage)
            };

            // Act
            BaseResponse actual = _roadMapsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.UpdateAsync(_roadMap), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            RoadMapData request = new()
            {
                Id = _roadMap.Id,
                CreatedOn = Timestamp.FromDateTime(_roadMap.CreatedOn),
                StaffId = _roadMap.StaffId,
                Status = (int)_roadMap.Status,
                Tasks = "Test update"
            };
            _roadMap.Tasks = request.Tasks;
            BaseResponse response = new()
            {
                Code = Code.UnknownError,
                ErrorMessage = "Road map was not updated"
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.UpdateAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Road map was not updated")
            };

            // Act
            BaseResponse actual = _roadMapsService.UpdateAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.UpdateAsync(_roadMap), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_update_existing_road_map_via_road_map_repository()
        {
            // Arrange
            RoadMapData request = new()
            {
                Id = _roadMap.Id,
                CreatedOn = Timestamp.FromDateTime(_roadMap.CreatedOn),
                StaffId = _roadMap.StaffId,
                Status = (int)_roadMap.Status,
                Tasks = _roadMap.Tasks
            };
            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            BaseResponse actual = _roadMapsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.DeleteAsync(_roadMap), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_db_update_exception()
        {
            // Arrange
            DbContextMock.ShouldThrowException = true;
            RoadMapData request = new()
            {
                Id = _roadMap.Id,
                CreatedOn = Timestamp.FromDateTime(_roadMap.CreatedOn),
                StaffId = _roadMap.StaffId,
                Status = (int)_roadMap.Status,
                Tasks = _roadMap.Tasks
            };
            BaseResponse response = new()
            {
                Code = Code.DbError,
                ErrorMessage = "An error occured while deleting road map"
            };

            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(DbContextMock.ExceptionMessage)
            };

            // Act
            BaseResponse actual = _roadMapsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.DeleteAsync(_roadMap), Times.Once);
        }

        [Test]
        public void DeleteAsync_should_handle_exception()
        {
            // Arrange
            DbContextMock.SaveChangesResult = 0;
            RoadMapData request = new()
            {
                Id = _roadMap.Id,
                CreatedOn = Timestamp.FromDateTime(_roadMap.CreatedOn),
                StaffId = _roadMap.StaffId,
                Status = (int)_roadMap.Status,
                Tasks = _roadMap.Tasks
            };
            _roadMap.Tasks = request.Tasks;
            BaseResponse response = new()
            {
                Code = Code.UnknownError,
                ErrorMessage = "Road map was not deleted"
            };
            LogData expectedLog = new()
            {
                CallSide = nameof(RoadMapsService),
                CallerMethodName = nameof(_roadMapsService.DeleteAsync),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Road map was not deleted")
            };

            // Act
            BaseResponse actual = _roadMapsService.DeleteAsync(request, null).Result;

            // Assert
            Assert.AreEqual(response, actual, "Response as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
            _roadMapRepositoryMock.Verify(m => m.DeleteAsync(_roadMap), Times.Once);
        }
    }
}
