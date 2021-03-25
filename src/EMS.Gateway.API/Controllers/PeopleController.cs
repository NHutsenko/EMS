using System;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.People;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : BaseApiController<PeopleController>
    {
        private readonly PeopleClient _peopleClient;

        public PeopleController(PeopleClient peopleClient, IEMSLogger<PeopleController> logger, IDateTimeUtil dateTimeUtil):
            base(logger, dateTimeUtil)
        {
            _peopleClient = peopleClient;
        }

        [HttpPost]
        public IActionResult Add([FromBody] PersonData request)
        {
            try
            {
                BaseResponse response = _peopleClient.AddAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(PeopleController),
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
                    CallSide = nameof(PeopleController),
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
        public IActionResult Update([FromBody] PersonData request)
        {
            try
            {
                BaseResponse response = _peopleClient.UpdateAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(PeopleController),
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
                    CallSide = nameof(PeopleController),
                    CallerMethodName = nameof(Update),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpPost("photo")]
        public IActionResult AddPhoto([FromBody] PhotoData request)
        {
            try
            {
                BaseResponse response = _peopleClient.AddPhotoAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(PeopleController),
                    CallerMethodName = nameof(AddPhoto),
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
                    CallSide = nameof(PeopleController),
                    CallerMethodName = nameof(AddPhoto),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpPost("contact")]
        public IActionResult AddContact([FromBody] ContactData request)
        {
            try
            {
                BaseResponse response = _peopleClient.AddContactAsync(request);
                LogData logData = new()
                {
                    CallSide = nameof(PeopleController),
                    CallerMethodName = nameof(AddContact),
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
                    CallSide = nameof(PeopleController),
                    CallerMethodName = nameof(AddContact),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return InternalServerError();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            Empty request = new();
            try
            {
                PeopleResponse response = _peopleClient.GetAll(request);
                LogData logData = new()
                {
                    CallSide = nameof(PeopleController),
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
                    CallSide = nameof(PeopleController),
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
        public IActionResult GetById([FromQuery] long personId)
        {
            ByPersonIdRequest request = new()
            {
                PersonId = personId
            };
            try
            {
                PersonResponse response = _peopleClient.GetById(request);
                LogData logData = new()
                {
                    CallSide = nameof(PeopleController),
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
                    CallSide = nameof(PeopleController),
                    CallerMethodName = nameof(GetById),
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
