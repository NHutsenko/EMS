using System;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.Holidays;


namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : BaseApiController<HolidaysController>
    {
        private readonly HolidaysClient _holidaysClient;

        public HolidaysController(HolidaysClient holidaysClient, IEMSLogger<HolidaysController> logger, IDateTimeUtil dateTimeUtil) : 
            base(logger, dateTimeUtil)
        {
            _holidaysClient = holidaysClient;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public IActionResult Add([FromBody] HolidayData request)
        {
            try
            {
                BaseResponse response = _holidaysClient.AddAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(Add),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };

                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(Add),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,HR")]
        public IActionResult Update([FromBody] HolidayData request)
        {
            try
            {
                BaseResponse response = _holidaysClient.UpdateAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(Update),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };

                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(Update),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,HR")]
        public IActionResult Delete([FromBody] HolidayData request)
        {
            try
            {
                BaseResponse response = _holidaysClient.DeleteAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(Delete),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };

                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(Delete),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpGet("all")]
        [Authorize]
        public IActionResult GetAll()
        {
            Empty request = new();
            try
            {
                HolidaysResponse response = _holidaysClient.GetAll(request);
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(GetAll),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };

                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(GetAll),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetByRangeDate([FromQuery] ByDateRangeRequest request)
        {
            try
            {
                HolidaysResponse response = _holidaysClient.GetByDateRange(request);
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(GetByRangeDate),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };

                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(HolidaysController),
                    CallerMethodName = nameof(GetByRangeDate),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }
    }
}
