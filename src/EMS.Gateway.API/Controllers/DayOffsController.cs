using System;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.DayOffs;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayOffsController : BaseApiController
    {
        private readonly DayOffsClient _dayOffsClient;
        private readonly IEMSLogger<DayOffsController> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public DayOffsController(DayOffsClient dayOffsClient, IEMSLogger<DayOffsController> logger, IDateTimeUtil dateTimeUtil)
        {
            _dateTimeUtil = dateTimeUtil;
            _dayOffsClient = dayOffsClient;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetByPersonId([FromQuery] ByPersonIdRequest request)
        {
            try
            {
                DayOffsResponse response = _dayOffsClient.GetByPersonId(request);
                LogData logData = new()
                {
                    CallSide = nameof(DayOffsController),
                    CallerMethodName = nameof(GetByPersonId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);
                return Ok(response);
            }
            catch(Exception ex)
            {
                LogData logData = new()
                {
                    CallSide = nameof(DayOffsController),
                    CallerMethodName = nameof(GetByPersonId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpGet("range")]
        public IActionResult GetByPersonIdAndDateRange([FromQuery] ByPersonIdAndDateRangeRequest request)
        {
            try
            {
                DayOffsResponse response = _dayOffsClient.GetByPersonIdAndDateRange(request);
                LogData logData = new()
                {
                    CallSide = nameof(DayOffsController),
                    CallerMethodName = nameof(GetByPersonIdAndDateRange),
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
                    CallSide = nameof(DayOffsController),
                    CallerMethodName = nameof(GetByPersonIdAndDateRange),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] DayOffData request)
        {
            try
            {
                BaseResponse response = _dayOffsClient.AddAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(DayOffsController),
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
                    CallSide = nameof(DayOffsController),
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
        public IActionResult Update([FromBody] DayOffData request)
        {
            try
            {
                BaseResponse response = _dayOffsClient.UpdateAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(DayOffsController),
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
                    CallSide = nameof(DayOffsController),
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
        public IActionResult Delete([FromBody] DayOffData request)
        {
            try
            {
                BaseResponse response = _dayOffsClient.DeleteAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(DayOffsController),
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
                    CallSide = nameof(DayOffsController),
                    CallerMethodName = nameof(Delete),
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
