using System;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.Positions;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly PositionsClient _positionsClient;
        private readonly IEMSLogger<PositionsController> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public PositionsController(PositionsClient positionsClient, IEMSLogger<PositionsController> logger, IDateTimeUtil dateTimeUtil)
        {
            _positionsClient = positionsClient;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }

        [HttpPost]
        public IActionResult Add(PositionData request)
        {
            try
            {
                BaseResponse response = _positionsClient.AddAsync(request);

                LogData logData = new()
                {
                    CallSide = nameof(PositionsController),
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
                    CallSide = nameof(PositionsController),
                    CallerMethodName = nameof(Add),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while sending request");
            }
        }

        [HttpPut]
        public IActionResult Update(PositionData request)
        {
            try
            {
                BaseResponse response = _positionsClient.UpdateAsync(request);

                LogData logData = new()
                {
                    CallSide = nameof(PositionsController),
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
                    CallSide = nameof(PositionsController),
                    CallerMethodName = nameof(Update),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while sending request");
            }
        }

        [HttpDelete]
        public IActionResult Delete(PositionData request)
        {
            try
            {
                BaseResponse response = _positionsClient.DeleteAsync(request);

                LogData logData = new()
                {
                    CallSide = nameof(PositionsController),
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
                    CallSide = nameof(PositionsController),
                    CallerMethodName = nameof(Delete),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while sending request");
            }
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            Empty request = new();
            try
            {
                PositionsResponse response = _positionsClient.GetAll(request);

                LogData logData = new()
                {
                    CallSide = nameof(PositionsController),
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
                    CallSide = nameof(PositionsController),
                    CallerMethodName = nameof(GetAll),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while sending request");
            }
        }

        [HttpGet]
        public IActionResult GetById([FromQuery] long id)
        {
            PositionRequest request = new()
            {
                PositionId = id
            };
            try
            {
                PositionResponse response = _positionsClient.GetById(request);

                LogData logData = new()
                {
                    CallSide = nameof(PositionsController),
                    CallerMethodName = nameof(GetById),
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
                    CallSide = nameof(PositionsController),
                    CallerMethodName = nameof(GetById),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };

                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while sending request");
            }
        }
    }
}
