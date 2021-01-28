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
            _dayOff1 = new DayOff
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

            _dayOffRepository = new DAL.Repositories.DayOffRepository(_dbContext);
            _dayOffsService = new DayOffsService(_dayOffRepository, _logger, _dateTimeUtil);
        }

        [Test]
        public void GetByPersionId_should_return_all_day_offs_by_specified_person()
        {
            // Arrange
            DayOffsByPersonRequest request = new DayOffsByPersonRequest
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
    }
}
