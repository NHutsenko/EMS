using System;
using EMS.Common.ControllerExtension;
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
    public class MotivationModificatorController : BaseApiController<MotivationModificatorController>
    {
        private readonly MotivationModificatorsClient _motivationModificatorsClient;

        public MotivationModificatorController(MotivationModificatorsClient client,
            IEMSLogger<MotivationModificatorController> logger, IDateTimeUtil dateTimeUtil): 
                base(logger, dateTimeUtil)
        {
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
                    CallSide = nameof(MotivationModificatorController),
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
        public IActionResult Update([FromBody] MotivationModificatorData request)
        {
            try
            {
                BaseResponse response = _motivationModificatorsClient.UpdateAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(MotivationModificatorController),
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
                    CallSide = nameof(MotivationModificatorController),
                    CallerMethodName = nameof(Update),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
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
                    CallSide = nameof(MotivationModificatorController),
                    CallerMethodName = nameof(GetBystaffId),
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
