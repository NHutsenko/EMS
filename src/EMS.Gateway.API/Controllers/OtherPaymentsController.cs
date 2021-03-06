﻿using System;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.OtherPayments;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherPaymentsController : BaseApiController<OtherPaymentsController>
    {
        private readonly OtherPaymentsClient _otherPaymentsClient;

        public OtherPaymentsController(OtherPaymentsClient client,
            IEMSLogger<OtherPaymentsController> logger,
            IDateTimeUtil dateTimeUtil) : base(logger, dateTimeUtil)
        {
            _otherPaymentsClient = client;
        }

        [HttpPost]
        [Authorize("Admin,Manager")]
        public IActionResult Add([FromBody] OtherPaymentData request)
        {
            try
            {
                BaseResponse response = _otherPaymentsClient.AddAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(OtherPaymentsController),
                    CallerMethodName = nameof(Add),
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
                    CallSide = nameof(OtherPaymentsController),
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
        [Authorize("Admin,Manager")]
        public IActionResult Update([FromBody] OtherPaymentData request)
        {
            try
            {
                BaseResponse response = _otherPaymentsClient.UpdateAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(OtherPaymentsController),
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
                    CallSide = nameof(OtherPaymentsController),
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
        [Authorize("Admin,Manager")]
        public IActionResult Delete([FromBody] OtherPaymentData request)
        {
            try
            {
                BaseResponse response = _otherPaymentsClient.DeleteAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(OtherPaymentsController),
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
                    CallSide = nameof(OtherPaymentsController),
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
        public IActionResult GetByPersonId([FromQuery] ByPersonIdRequest request)
        {
            try
            {
                OtherPaymentsResponse response = _otherPaymentsClient.GetByPersonId(request);
                LogData logData = new()
                {
                    CallSide = nameof(OtherPaymentsController),
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
                    CallSide = nameof(OtherPaymentsController),
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
        [Authorize]
        public IActionResult GetByPersonIdAndDateRange([FromQuery] ByPersonIdAndDateRangeRequest request)
        {
            try
            {
                OtherPaymentsResponse response = _otherPaymentsClient.GetByPersonIdAndDateRange(request);
                LogData logData = new()
                {
                    CallSide = nameof(OtherPaymentsController),
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
                    CallSide = nameof(OtherPaymentsController),
                    CallerMethodName = nameof(GetByPersonIdAndDateRange),
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
