using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EMS.Core.API.Models;
using EMS.Core.API.Services;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class SalaryServiceTest : BaseUnitTest
    {
        public Staff _staff1;
        public Position _position1;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
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

            _salaryService = new SalaryService(null, _staffRepository, _dayOffRepository, _holidaysRepository, _positionsRepository, _motivationModificatorRepository);
        }

        [Test]
        public void GetSalary_should_return_month_salary_only_with_work_days()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                CurrentSalary = 1680,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };

            SalaryRequest request = new SalaryRequest();
            request.StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            request.EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1));

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;
            SalaryResponse actual = response.SalaryResponse.First();

            // Assert
            Assert.AreEqual(expected.CurrentSalary, actual.CurrentSalary, "Salary calculated as expected");
            Assert.AreEqual(expected.PersonId, actual.PersonId, "Employee id returned as expected");
            Assert.AreEqual(expected.CurrentPosition, actual.CurrentPosition, "Employee actual position returned as expected");
            Assert.AreEqual(expected.StartedOn.ToDateTime(), actual.StartedOn.ToDateTime(), "Date of start work returned as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_paid_holidays()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                CurrentSalary = 1680,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
            };
            Holiday holiday = new Holiday
            {
                HolidayDate = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Id = 1,
                Description = "Test"
            };
            _dbContext.Holidays.Add(holiday);

            SalaryRequest request = new SalaryRequest();
            request.StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            request.EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1));
            request.ManagerId = 1;

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;
            SalaryResponse actual = response.SalaryResponse.First();

            // Assert
            Assert.AreEqual(expected.CurrentSalary, actual.CurrentSalary, "Salary calculated as expected");
            Assert.AreEqual(expected.PersonId, actual.PersonId, "Employee id returned as expected");
            Assert.AreEqual(expected.CurrentPosition, actual.CurrentPosition, "Employee actual position returned as expected");
            Assert.AreEqual(expected.StartedOn.ToDateTime(), actual.StartedOn.ToDateTime(), "Date of start work returned as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_unpaid_holidays()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                CurrentSalary = 1680,
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

            SalaryRequest request = new SalaryRequest();
            request.StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            request.EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1));

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;
            SalaryResponse actual = response.SalaryResponse.First();

            // Assert
            Assert.AreEqual(expected.CurrentSalary, actual.CurrentSalary, "Salary calculated as expected");
            Assert.AreEqual(expected.PersonId, actual.PersonId, "Employee id returned as expected");
            Assert.AreEqual(expected.CurrentPosition, actual.CurrentPosition, "Employee actual position returned as expected");
            Assert.AreEqual(expected.StartedOn.ToDateTime(), actual.StartedOn.ToDateTime(), "Date of start work returned as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_paid_dayoff()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                CurrentSalary = 1680,
                StartedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime().ToUniversalTime())
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

            _dbContext.DaysOff.Add(dayOff);

            SalaryRequest request = new SalaryRequest();
            request.StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            request.EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1));

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;
            SalaryResponse actual = response.SalaryResponse.First();

            // Assert
            Assert.AreEqual(expected.CurrentSalary, actual.CurrentSalary, "Salary calculated as expected");
            Assert.AreEqual(expected.PersonId, actual.PersonId, "Employee id returned as expected");
            Assert.AreEqual(expected.CurrentPosition, actual.CurrentPosition, "Employee actual position returned as expected");
            Assert.AreEqual(expected.StartedOn.ToDateTime(), actual.StartedOn.ToDateTime(), "Date of start work returned as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_unpaid_dayoff()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                CurrentSalary = 1600,
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

            _dbContext.DaysOff.Add(dayOff);

            SalaryRequest request = new SalaryRequest();
            request.StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            request.EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1));

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;
            SalaryResponse actual = response.SalaryResponse.First();

            // Assert
            Assert.AreEqual(expected.CurrentSalary, actual.CurrentSalary, "Salary calculated as expected");
            Assert.AreEqual(expected.PersonId, actual.PersonId, "Employee id returned as expected");
            Assert.AreEqual(expected.CurrentPosition, actual.CurrentPosition, "Employee actual position returned as expected");
            Assert.AreEqual(expected.StartedOn.ToDateTime(), actual.StartedOn.ToDateTime(), "Date of start work returned as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_partly_unpayed_dayoff()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                CurrentSalary = 1640,
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

            _dbContext.DaysOff.Add(dayOff);

            SalaryRequest request = new SalaryRequest();
            request.StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            request.EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1));

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;
            SalaryResponse actual = response.SalaryResponse.First();

            // Assert
            Assert.AreEqual(expected.CurrentSalary, actual.CurrentSalary, "Salary calculated as expected");
            Assert.AreEqual(expected.PersonId, actual.PersonId, "Employee id returned as expected");
            Assert.AreEqual(expected.CurrentPosition, actual.CurrentPosition, "Employee actual position returned as expected");
            Assert.AreEqual(expected.StartedOn.ToDateTime(), actual.StartedOn.ToDateTime(), "Date of start work returned as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_partly_payed_dayoff()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                CurrentSalary = 1680,
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

            _dbContext.DaysOff.Add(dayOff);

            SalaryRequest request = new SalaryRequest();
            request.StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            request.EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1));

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;
            SalaryResponse actual = response.SalaryResponse.First();

            // Assert
            Assert.AreEqual(expected.CurrentSalary, actual.CurrentSalary, "Salary calculated as expected");
            Assert.AreEqual(expected.PersonId, actual.PersonId, "Employee id returned as expected");
            Assert.AreEqual(expected.CurrentPosition, actual.CurrentPosition, "Employee actual position returned as expected");
            Assert.AreEqual(expected.StartedOn.ToDateTime(), actual.StartedOn.ToDateTime(), "Date of start work returned as expected");
        }

        [Test]
        public void GetSalary_should_return_month_salary_with_motivation_modificator()
        {
            // Arrange
            SalaryResponse expected = new SalaryResponse
            {
                CurrentPosition = _position1.Id,
                PersonId = _staff1.PersonId.GetValueOrDefault(),
                CurrentSalary = 840,
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


            SalaryRequest request = new SalaryRequest();
            request.StartDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            request.EndDate = Timestamp.FromDateTime(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddDays(-1));

            // Act
            ISalaryResponse response = _salaryService.GetSalary(request, null).Result;
            SalaryResponse actual = response.SalaryResponse.First();

            // Assert
            Assert.AreEqual(expected.CurrentSalary, actual.CurrentSalary, "Salary calculated as expected");
            Assert.AreEqual(expected.PersonId, actual.PersonId, "Employee id returned as expected");
            Assert.AreEqual(expected.CurrentPosition, actual.CurrentPosition, "Employee actual position returned as expected");
            Assert.AreEqual(expected.StartedOn.ToDateTime(), actual.StartedOn.ToDateTime(), "Date of start work returned as expected");
        }
    }
}
