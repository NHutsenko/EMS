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
    public class SalaryServiceTest : BaseUnitTest<SalaryService>
    {
        public Staff _staff1;
        public Position _position1;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new SalaryService(null, null, null, null, null, null, null, null));
            DbContextMock.ShouldThrowException = false;
            _position1 = new Position
            {
                Id = 1,
                HourRate = 10,
                Name = "Test"
            };

            _dbContext.Positions.Add(_position1);

            _staff1 = new Staff
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                PositionId = _position1.Id,
                PersonId = 1,
                ManagerId = 1
            };

            _dbContext.Staff.Add(_staff1);

            _positionsRepository = new DAL.Repositories.PositionsRepository(_dbContext, _dateTimeUtil);
            _staffRepository = new DAL.Repositories.StaffRepository(_dbContext);
            _dayOffRepository = new DAL.Repositories.DayOffRepository(_dbContext);
            _holidaysRepository = new DAL.Repositories.HolidaysRepository(_dbContext, _dateTimeUtil);
            _motivationModificatorRepository = new DAL.Repositories.MotivationModificatorRepository(_dbContext, _dateTimeUtil);
            _otherPaymentsRepository = new DAL.Repositories.OtherPaymentsRepository(_dbContext, _dateTimeUtil); 

            _salaryService = new SalaryService(_logger, 
                _staffRepository, 
                _dayOffRepository, 
                _holidaysRepository, 
                _positionsRepository, 
                _motivationModificatorRepository,
                _otherPaymentsRepository,
                _dateTimeUtil);
        }

        [Test]
        public void GetSalary_should_return_month_salary_only_with_work_days()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 1680,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(SalaryService),
                CallerMethodName = nameof(_salaryService.GetSalary),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = salaryResponse
            };

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetSalary_should_return_month_salary_for_preson_who_started_work_later_than_start_period_date()
        {
            // Arrange
            _staff1.CreatedOn = new DateTime(2021, 1, 15, 12, 00, 00);
            _dbContext.Staff.Update(_staff1);
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 880,
                StartedOn = Timestamp.FromDateTime(new DateTime(2021, 1, 15, 12, 00, 00).ToUniversalTime())
            };

            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(SalaryService),
                CallerMethodName = nameof(_salaryService.GetSalary),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = salaryResponse
            };

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_paid_holidays()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 1680,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            Holiday holiday = new Holiday
            {
                HolidayDate = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Id = 1,
                Description = "Test"
            };
            _dbContext.Holidays.Add(holiday);
            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1)),
                ManagerId = 1
            };

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_unpaid_holidays()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 1680,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            Holiday holiday = new Holiday
            {
                HolidayDate = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ToDoDate = new DateTime(2021, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                Id = 1,
                Description = "Test"
            };
            _dbContext.Holidays.Add(holiday);

            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_paid_dayoff()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 1680,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime()),
            };
            DayOff dayOff = new DayOff
            {
                DayOffType = Enums.DayOffType.Vacation,
                IsPaid = true,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Hours = 8,
                Id = 1,
                CreatedOn = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            expected.DayOffs.Add(new DayOffData
            {
                DayOffType = (int)dayOff.DayOffType,
                Hours = dayOff.Hours
            });

            _dbContext.DaysOff.Add(dayOff);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };
            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_unpaid_dayoff()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 1600,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            DayOff dayOff = new DayOff
            {
                DayOffType = Enums.DayOffType.Vacation,
                IsPaid = false,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Hours = 8,
                Id = 1,
                CreatedOn = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            expected.DayOffs.Add(new DayOffData
            {
                DayOffType = (int)dayOff.DayOffType,
                Hours = dayOff.Hours
            });

            _dbContext.DaysOff.Add(dayOff);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };

            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_partly_unpaid_dayoff()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 1640,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            DayOff dayOff = new DayOff
            {
                DayOffType = Enums.DayOffType.Vacation,
                IsPaid = false,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Hours = 4,
                Id = 1,
                CreatedOn = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            expected.DayOffs.Add(new DayOffData
            {
                DayOffType = (int)dayOff.DayOffType,
                Hours = dayOff.Hours
            });

            _dbContext.DaysOff.Add(dayOff);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };

            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_partly_payed_dayoff()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 1680,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            DayOff dayOff = new DayOff
            {
                DayOffType = Enums.DayOffType.Vacation,
                IsPaid = true,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Hours = 4,
                Id = 1,
                CreatedOn = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            expected.DayOffs.Add(new DayOffData
            {
                DayOffType = (int)dayOff.DayOffType,
                Hours = dayOff.Hours
            });

            _dbContext.DaysOff.Add(dayOff);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };
            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_motivation_modificator()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 840,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            MotivationModificator modificator = new MotivationModificator
            {
                Id = 1,
                ModValue = 0.5,
                StaffId = _staff1.Id
            };
            _dbContext.MotivationModificators.Add(modificator);
            _staff1.MotivationModificatorId = modificator.Id;


            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };

            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_other_payments()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                Salary = 1700,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            OtherPayment otherPayment = new OtherPayment
            {
                CreatedOn = new DateTime(2021, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                Id = 1,
                PersonId = _staff1.PersonId.Value,
                Value = 20
            };
            _dbContext.OtherPayments.Add(otherPayment);

            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };

            ISalaryResponse salaryResponse = new ISalaryResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            salaryResponse.SalaryResponse.Add(expected);

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(salaryResponse, response, "Calculated as expected");
        }

        [Test]
        public void GetSalary_should_handle_null_reference_exception()
        {
            // Arrange
            _dbContext.Positions.Remove(_position1);
            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };

            LogData expectedLog = new LogData
            {
                CallSide = nameof(SalaryService),
                CallerMethodName = nameof(_salaryService.GetSalary),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Object reference not set to an instance of an object.")
            };

            // Act
            ISalaryResponse actual = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(Code.DataError, actual.Status.Code, "Code returned as expected");
            Assert.AreEqual("Some data has not found (type: NullReferenceException)", actual.Status.ErrorMessage, "Error message as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetSalary_should_handle_base_exception()
        {
            // Arrange
            _dbContext.Positions = null;
            SalaryRequest request = new SalaryRequest
            {
                StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1))
            };
            LogData expectedLog = new LogData
            {
                CallSide = nameof(SalaryService),
                CallerMethodName = nameof(_salaryService.GetSalary),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception("Value cannot be null. (Parameter 'source')")
            };
            
            // Act
            ISalaryResponse actual = _salaryService.GetSalary(request, null).Result;

            // Assert
            Assert.AreEqual(Code.UnknownError, actual.Status.Code, "Code returned as expected");
            Assert.AreEqual("Value cannot be null. (Parameter 'source')", actual.Status.ErrorMessage, "Error message as expected");
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
