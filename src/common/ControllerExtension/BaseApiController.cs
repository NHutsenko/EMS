using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Common.ControllerExtension
{
    public class BaseApiController<T>: ControllerBase
	{
        private static string ErrorMessage => "An error occured while sending request";
        protected readonly IEMSLogger<T> _logger;
        protected readonly IDateTimeUtil _dateTimeUtil;

        public BaseApiController(IEMSLogger<T> logger, IDateTimeUtil dateTimeUtil)
        {
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }

        protected IActionResult InternalServerError()
        {
            return StatusCode(500, ErrorMessage);
        }
	}
}
