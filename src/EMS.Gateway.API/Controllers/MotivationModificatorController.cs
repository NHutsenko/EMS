using System;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.MotivationModificators;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivationModificatorController : ControllerBase
    {
        private readonly MotivationModificatorsClient _motivationModificatorsClient;
        private readonly IEMSLogger<MotivationModificatorController> _logger;
        private readonly IDateTimeUtil _datetimeUtil;

        public MotivationModificatorController(MotivationModificatorsClient client,
            IEMSLogger<MotivationModificatorController> logger, IDateTimeUtil dateTimeUtil)
        {
            _datetimeUtil = dateTimeUtil;
            _logger = logger;  
            _motivationModificatorsClient = client;
        }

        [HttpPost]
        public IActionResult  Add([FromBody] MotivationModificatorData request)
        {
            try
            {
                BaseResponse response = _motivationModificatorsClient.AddAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(MotivationModificatorController),
                    CallerMethodName = nameof(Add),
                    CreatedOn = _datetimeUtil.GetCurrentDateTime(),
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
                    CallSide = nameof(MotivationModificatorController),
                    CallerMethodName = nameof(Add),
                    CreatedOn = _datetimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while sending request");
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] MotivationModificatorData request)
        {
            try
            {
                BaseResponse response = _motivationModificatorsClient.UpdateAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(MotivationModificatorController),
                    CallerMethodName = nameof(Update),
                    CreatedOn = _datetimeUtil.GetCurrentDateTime(),
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
                    CallSide = nameof(MotivationModificatorController),
                    CallerMethodName = nameof(Update),
                    CreatedOn = _datetimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while sending request");
            }
        }

        [HttpGet]
        public IActionResult GetBystaffId([FromQuery] long staffId)
        {
            ByStaffIdRequest request = new()
            {
                StaffId = staffId
            };

            try
            {
                MotivationModificatorResponse response = _motivationModificatorsClient.GetByStaffId(request);
                LogData logData = new()
                {
                    CallSide = nameof(MotivationModificatorController),
                    CallerMethodName = nameof(GetBystaffId),
                    CreatedOn = _datetimeUtil.GetCurrentDateTime(),
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
                    CallSide = nameof(MotivationModificatorController),
                    CallerMethodName = nameof(GetBystaffId),
                    CreatedOn = _datetimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while sending request");
            }
        }
    }
}
