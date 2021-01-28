using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Protos;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Common.Logger.Models;

namespace EMS.Core.API.Services
{
    public class SalaryService : Salary.SalaryBase
    {
        private readonly IEMSLogger<SalaryService> _logger;
        private readonly IStaffRepository _staffRepository;
        private readonly IDayOffRepository _dayOffRepository;
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IPositionsRepository _positionsRepository;
        private readonly IMotivationModificatorRepository _motivationModificatorRepository;
        private readonly IOtherPaymentsRepository _otherPaymentsRepository;
        private readonly IDateTimeUtil _dateTimeUtil;

        public SalaryService(IEMSLogger<SalaryService> logger,
            IStaffRepository staffRepository,
            IDayOffRepository dayOffRepository,
            IHolidaysRepository holidaysRepository,
            IPositionsRepository positionsRepository,
            IMotivationModificatorRepository motivationModificatorRepository,
            IOtherPaymentsRepository otherPaymentsRepository,
            IDateTimeUtil dateTimeUtil)
        {
            _logger = logger;
            _staffRepository = staffRepository;
            _dayOffRepository = dayOffRepository;
            _holidaysRepository = holidaysRepository;
            _positionsRepository = positionsRepository;
            _motivationModificatorRepository = motivationModificatorRepository;
            _otherPaymentsRepository = otherPaymentsRepository;
            _dateTimeUtil = dateTimeUtil;
        }

        public override Task<ISalaryResponse> GetSalary(SalaryRequest request, ServerCallContext context)
        {
            ISalaryResponse response = new ISalaryResponse()
            {
                Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
            };
            try
            {
                IQueryable<Staff> staffs = request.ManagerId == 0 ? _staffRepository.GetAll() : _staffRepository.GetByManagerId(request.ManagerId);
                IQueryable<IGrouping<long?, Staff>> groupedStaff = staffs.Where(e => e.PersonId.HasValue)
                    .GroupBy(e => e.PersonId);
                foreach (IGrouping<long?, Staff> data in groupedStaff)
                {
                    SalaryResponse salaryResponse = CalculateCurrentSalary(data, request.StartDate.ToDateTime(), request.EndDate.ToDateTime());
                    salaryResponse = CalculateOtherPayments(salaryResponse, request.StartDate.ToDateTime(), request.EndDate.ToDateTime());
                    response.SalaryResponse.Add(salaryResponse);
                }
            }
            catch (NullReferenceException nrex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(SalaryService),
                    CallerMethodName = nameof(GetSalary),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = nrex
                };

                _logger.AddErrorLog(logData);
                response.Status.ErrorMessage = $"Some data has not found (type: {nrex.GetType().Name})";
                response.Status.Code = Code.DataError;
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(SalaryService),
                    CallerMethodName = nameof(GetSalary),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                response.Status.ErrorMessage = ex.Message;
                response.Status.Code = Code.UnknownError;
            }

            LogData log = new LogData
            {
                CallSide = nameof(SalaryService),
                CallerMethodName = nameof(GetSalary),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            _logger.AddLog(log);

            return Task.FromResult(response);
        }

        private SalaryResponse CalculateCurrentSalary(IGrouping<long?, Staff> staff, DateTime startDate, DateTime endDate)
        {
            IQueryable<DayOff> dayOffs = _dayOffRepository.GetByDateRangeAndPersonId(startDate, endDate, staff.First().PersonId.Value);
            IQueryable<Holiday> holidays = _holidaysRepository.GetByDateRange(startDate, endDate);
            SalaryResponse response = new SalaryResponse
            {
                StartedOn = Timestamp.FromDateTime(staff.First().CreatedOn.ToUniversalTime())
            };
            double workHours = GetWorkHours();

            for (DateTime current = startDate.Date; current.Date <= endDate.Date; current = current.AddDays(1))
            {
                bool todoDay = (current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday)
                    && holidays.Any(e => e.ToDoDate.HasValue && e.ToDoDate.Value.Date == current.Date);

                bool workDay = current.DayOfWeek != DayOfWeek.Saturday
                    && current.DayOfWeek != DayOfWeek.Sunday
                    && !holidays.Any(e => e.HolidayDate.Date == current.Date);

                Staff currentStaff = staff.OrderByDescending(e => e.CreatedOn).FirstOrDefault(e => e.CreatedOn.Date <= current.Date);
                if (currentStaff is not null)
                {
                    Position position = _positionsRepository.Get(currentStaff.PositionId);
                    MotivationModificator modificator = _motivationModificatorRepository.GetByStaffId(currentStaff.MotivationModificatorId);
                    double rate = modificator != null ? position.HourRate * modificator.ModValue : position.HourRate;
                    response.CurrentPosition = position.Id;
                    response.PersonId = currentStaff.PersonId.GetValueOrDefault();
                    if (workDay || todoDay)
                    {
                        if (!dayOffs.Any(e => e.CreatedOn.Date == current.Date))
                        {
                            response.Salary += workHours * rate;
                        }
                        else
                        {
                            DayOff dayOff = dayOffs.First(e => e.CreatedOn.Date == current.Date);
                            response.DayOffs.Add(new DayOffInfo
                            {
                                DayOffType = (int)dayOff.DayOffType,
                                Hours = dayOff.Hours
                            });
                            if (dayOff.IsPaid)
                            {
                                if (dayOff.Hours < workHours)
                                {
                                    response.Salary += dayOff.Hours * rate + (workHours - dayOff.Hours) * rate;
                                }
                                else
                                {
                                    response.Salary += dayOff.Hours * rate;
                                }
                            }
                            else
                            {
                                response.Salary += (workHours - dayOff.Hours) * rate;
                            }
                        }
                    }
                    else if (holidays.Any(e => e.HolidayDate.Date == current.Date && !e.ToDoDate.HasValue))
                    {
                        response.Salary += workHours * rate;
                    }
                }
            }
            return response;
        }

        private SalaryResponse CalculateOtherPayments(SalaryResponse calculatedSalary, DateTime start, DateTime end)
        {
            IQueryable<OtherPayment> otherPayments = _otherPaymentsRepository.GetByPersonIdAndDateRange(calculatedSalary.PersonId, start, end);

            foreach(OtherPayment otherPayment in otherPayments)
            {
                calculatedSalary.Salary += otherPayment.Value;
            }

            return calculatedSalary;
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
