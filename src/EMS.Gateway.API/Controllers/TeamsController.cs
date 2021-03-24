using System;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : BaseApiController
    {
        private readonly Teams.TeamsClient _teamsClient;
        private readonly IEMSLogger<TeamsController> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public TeamsController(Teams.TeamsClient teamsClient, IEMSLogger<TeamsController> logger, IDateTimeUtil dateTimeUtil)
        {
            _teamsClient = teamsClient;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }

        [HttpPost]
        public IActionResult Add([FromBody] TeamData teamData)
        {
            try
            {
                BaseResponse response = _teamsClient.AddAsync(teamData);
                LogData logData = new()
                {
                    CallerMethodName = nameof(Add),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = response
                };
                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallerMethodName = nameof(Add),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] TeamData teamData)
        {
            try
            {
                BaseResponse response = _teamsClient.UpdateAsync(teamData);
                LogData logData = new()
                {
                    CallerMethodName = nameof(Update),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = response
                };
                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallerMethodName = nameof(Update),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] TeamData teamData)
        {
            try
            {
                BaseResponse response = _teamsClient.DeleteAsync(teamData);
                LogData logData = new()
                {
                    CallerMethodName = nameof(Delete),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = response
                };
                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallerMethodName = nameof(Delete),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                TeamsResponse response = _teamsClient.GetAll(new Empty());
                LogData logData = new()
                {
                    CallerMethodName = nameof(GetAll),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = new Empty(),
                    Response = response
                };
                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new()
                {
                    CallerMethodName = nameof(GetAll),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = new Empty(),
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpGet]
        public IActionResult GetById([FromQuery] long teamId)
        {
            TeamRequest request = new()
            {
                Id = teamId
            };
            try
            {
                TeamResponse response = _teamsClient.GetById(request);
                LogData logData = new()
                {
                    CallerMethodName = nameof(GetById),
                    CallSide = nameof(TeamsController),
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
                    CallerMethodName = nameof(GetById),
                    CallSide = nameof(TeamsController),
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
