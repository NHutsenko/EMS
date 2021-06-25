using System;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.Staffs;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : BaseApiController<StaffController>
    {
        private readonly StaffsClient _staffsClient;

        public StaffController(StaffsClient staffsClient, IEMSLogger<StaffController> logger, IDateTimeUtil dateTimeUtil) :
            base(logger, dateTimeUtil)
        {
            _staffsClient = staffsClient;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR,Manager")]
        public IActionResult Add([FromBody] StaffData request)
        {
            try
            {
                BaseResponse response = _staffsClient.AddAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(StaffController),
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
                    CallSide = nameof(StaffController),
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
        [Authorize(Roles = "Admin,HR,Manager")]
        public IActionResult Update([FromBody] StaffData request)
        {
            try
            {
                BaseResponse response = _staffsClient.UpdateAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(StaffController),
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
                    CallSide = nameof(StaffController),
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
        [Authorize(Roles = "Admin,HR,Manager")]
        public IActionResult Delete([FromBody] StaffData request)
        {
            try
            {
                BaseResponse response = _staffsClient.DeleteAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(StaffController),
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
                    CallSide = nameof(StaffController),
                    CallerMethodName = nameof(Delete),
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
        public IActionResult GetAll()
        {
            Empty request = new();
            try
            {
                StaffResponse response = _staffsClient.GetAll(request);
                LogData logData = new()
                {
                    CallSide = nameof(StaffController),
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
                    CallSide = nameof(StaffController),
                    CallerMethodName = nameof(GetAll),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpGet("person")]
        [Authorize]
        public IActionResult GetByPersonId([FromQuery] ByPersonIdRequest request)
        {
            try
            {
                StaffResponse response = _staffsClient.GetByPersonId(request);
                LogData logData = new()
                {
                    CallSide = nameof(StaffController),
                    CallerMethodName = nameof(GetByPersonId),
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
                    CallSide = nameof(StaffController),
                    CallerMethodName = nameof(GetByPersonId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpGet("manager")]
        [Authorize]
        public IActionResult GetByManagerId([FromQuery] ByPersonIdRequest request)
        {
            try
            {
                StaffResponse response = _staffsClient.GetByManagerId(request);
                LogData logData = new()
                {
                    CallSide = nameof(StaffController),
                    CallerMethodName = nameof(GetByManagerId),
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
                    CallSide = nameof(StaffController),
                    CallerMethodName = nameof(GetByManagerId),
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
