using System;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.RoadMaps;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadMapsController : BaseApiController<RoadMapsController>
    {
        private readonly RoadMapsClient _roadMapsClient;
        public RoadMapsController(RoadMapsClient roadMapsClient, IEMSLogger<RoadMapsController> logger, IDateTimeUtil dateTimeUtil) : base(logger, dateTimeUtil)
        {
            _roadMapsClient = roadMapsClient;
        }

        [HttpGet]
        public IActionResult GetByStaffId([FromQuery] ByStaffRequest request)
        {
            try
            {
                RoadMapResponse response = _roadMapsClient.GetByStaffId(request);
                LogData logData = new()
                {
                    CallSide = nameof(RoadMapsController),
                    CallerMethodName = nameof(GetByStaffId),
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
                    CallSide = nameof(RoadMapsController),
                    CallerMethodName = nameof(GetByStaffId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpPost]
        public IActionResult Add([FromQuery] RoadMapData request)
        {
            try
            {
                BaseResponse response = _roadMapsClient.AddAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(RoadMapsController),
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
                    CallSide = nameof(RoadMapsController),
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
        public IActionResult Update([FromQuery] RoadMapData request)
        {
            try
            {
                BaseResponse response = _roadMapsClient.UpdateAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(RoadMapsController),
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
                    CallSide = nameof(RoadMapsController),
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
        public IActionResult Delete([FromQuery] RoadMapData request)
        {
            try
            {
                BaseResponse response = _roadMapsClient.DeleteAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(RoadMapsController),
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
                    CallSide = nameof(RoadMapsController),
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
