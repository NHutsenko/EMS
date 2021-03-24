using Microsoft.AspNetCore.Mvc;

namespace EMS.Common.ControllerExtension
{
    public class BaseApiController: ControllerBase
	{
        private static string ErrorMessage => "An error occured while sending request";
        public IActionResult InternalServerError()
        {
            return StatusCode(500, ErrorMessage);
        }
	}
}
