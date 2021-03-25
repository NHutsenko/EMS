using System;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Mvc;
using static EMS.Common.Protos.Salary;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : BaseApiController<SalaryController>
    {
        private readonly SalaryClient _salaryClient;

        public SalaryController(SalaryClient salaryClient,
            IEMSLogger<SalaryController> logger,
            IDateTimeUtil dateTimeUtil) : base(logger, dateTimeUtil)
        {
            _salaryClient = salaryClient;
        }

        [HttpGet]
        public IActionResult GetSalary([FromQuery] SalaryRequest request)
        {
            try
            {
                ISalaryResponse response = _salaryClient.GetSalary(request);
                LogData logData = new LogData
                {
                    CallSide = nameof(SalaryController),
                    CallerMethodName = nameof(GetSalary),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(SalaryController),
                    CallerMethodName = nameof(GetSalary),
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
