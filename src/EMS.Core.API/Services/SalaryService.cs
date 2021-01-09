using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EMS.Core.API.Services
{
    public class SalaryService : Salary.SalaryBase
    {
        private readonly ILogger<SalaryService> _logger;
        private readonly IStaffRepository _staffRepository;
        private readonly IDayOffRepository _dayOffRepository;
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IPositionsRepository _positionsRepository;

        public SalaryService(ILogger<SalaryService> logger,
            IStaffRepository staffRepository,
            IDayOffRepository dayOffRepository,
            IHolidaysRepository holidaysRepository,
            IPositionsRepository positionsRepository)
        {
            _logger = logger;
            _staffRepository = staffRepository;
            _dayOffRepository = dayOffRepository;
            _holidaysRepository = holidaysRepository;
            _positionsRepository = positionsRepository;
        }

        public override Task<ISalaryResponse> GetSalary(SalaryRequest request, ServerCallContext context)
        {
            ISalaryResponse response = new ISalaryResponse();

            IQueryable<Staff> staffs = request.ManagerId == 0 ? _staffRepository.GetAll() : _staffRepository.GetByManagerId(request.ManagerId);
            IQueryable<IGrouping<long?, Staff>> groupedStaff = staffs.Where(e => e.PersonId.HasValue)
                .GroupBy(e => e.PersonId);
            foreach (IGrouping<long?, Staff> data in groupedStaff)
            {
                response.SalaryResponse.Add(CalculateCurrentSalary(data, request.StartDate.ToDateTime(), request.EndDate.ToDateTime()));
            }

            return Task.FromResult(response);
        }


        // TODO: Add to calculation other payments and grademods
        private SalaryResponse CalculateCurrentSalary(IGrouping<long?, Staff> staff, DateTime startDate, DateTime endDate)
        {
            IQueryable<DayOff> dayOffs = _dayOffRepository.GetByDateRangeAndPersonId(startDate, endDate, staff.First().PersonId.Value);
            IQueryable<Holiday> holidays = _holidaysRepository.GetByDateRange(startDate, endDate);
            SalaryResponse response = new SalaryResponse();
            response.StartedOn = Timestamp.FromDateTime(staff.First().CreatedOn.ToUniversalTime());
            double workHours = GetWorkHours();

            for (DateTime current = startDate; current <= endDate; current = current.AddDays(1))
            {
                bool todoDay = (current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday)
                    && holidays.Any(e => e.ToDoDate.HasValue && e.ToDoDate.Value.Date == current.Date);

                bool notHoliday = current.DayOfWeek != DayOfWeek.Saturday
                    && current.DayOfWeek != DayOfWeek.Sunday
                    && !holidays.Any(e => e.HolidayDate.Date == current.Date);
                if (notHoliday || todoDay)
                {
                    Staff currentStaff = staff.OrderByDescending(e => e.CreatedOn).FirstOrDefault(e => e.CreatedOn <= current);
                    if (currentStaff is not null)
                    {
                        Position position = _positionsRepository.Get(currentStaff.PositionId);
                        response.CurrentPosition = position.Id;
                        if (!dayOffs.Any(e => e.CreatedOn.Date == current.Date))
                        {
                            response.CurrentSalary += workHours * position.HourRate;
                        }
                        else
                        {
                            DayOff dayOff = dayOffs.First(e => e.CreatedOn.Date == current.Date);
                            response.DayOffs.Add(new DayOffData
                            {
                                DayOffType = (int)dayOff.DayOffType,
                                Hours = dayOff.Hours
                            });
                            if (dayOff.IsPaid)
                            {
                                if (dayOff.Hours < workHours)
                                {
                                    response.CurrentSalary += dayOff.Hours * position.HourRate + (workHours - dayOff.Hours) * position.HourRate;
                                }
                                else
                                {
                                    response.CurrentSalary += dayOff.Hours * position.HourRate;
                                }
                            }
                        }
                    }
                }

            }
            return response;
        }

        private static double GetWorkHours()
        {
            bool parsed = double.TryParse(Environment.GetEnvironmentVariable("MaxWorkHours"), out double workHours);
            if (!parsed)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                _ = double.TryParse(configuration["Settings:MaxWorkHours"], out workHours);
            }
            return workHours;
        }
    }
}
