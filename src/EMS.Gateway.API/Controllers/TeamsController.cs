using System;
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
    public class TeamsController : ControllerBase
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
                LogData logData = new LogData
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
                LogData logData = new LogData
                {
                    CallerMethodName = nameof(Add),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while making request");
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] TeamData teamData)
        {
            try
            {
                BaseResponse response = _teamsClient.UpdateAsync(teamData);
                LogData logData = new LogData
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
                LogData logData = new LogData
                {
                    CallerMethodName = nameof(Update),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while making request");
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] TeamData teamData)
        {
            try
            {
                BaseResponse response = _teamsClient.DeleteAsync(teamData);
                LogData logData = new LogData
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
            catch(Exception ex)
            {
                LogData logData = new LogData
                {
                    CallerMethodName = nameof(Delete),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = teamData,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while making request");
            }
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                TeamsResponse response = _teamsClient.GetAll(new Empty());
                LogData logData = new LogData
                {
                    CallerMethodName = nameof(GetAll),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = new Empty(),
                    Response = response
                };
                _logger.AddErrorLog(logData);
                return Ok(response);
            }
            catch(Exception ex)
            {
                LogData logData = new LogData
                {
                    CallerMethodName = nameof(GetAll),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = new Empty(),
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while making request");
            }
        }

        [HttpGet]
        public IActionResult GetById([FromQuery] long teamId)
        {
            try
            {
                TeamsResponse response = _teamsClient.GetAll(new Empty());
                LogData logData = new LogData
                {
                    CallerMethodName = nameof(GetAll),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = new Empty(),
                    Response = response
                };
                _logger.AddErrorLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallerMethodName = nameof(GetAll),
                    CallSide = nameof(TeamsController),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = new Empty(),
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return StatusCode(500, "An error occured while making request");
            }
        }
    }
}
