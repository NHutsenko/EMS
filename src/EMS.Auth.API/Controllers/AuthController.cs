using System.Threading.Tasks;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models.RequestModels;
using EMS.Auth.API.Models.ResponseModels;
using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController<AuthController>
    {
        private readonly IAuthService _authService;
        public AuthController(IEMSLogger<AuthController> logger, IDateTimeUtil dateTimeUtil, IAuthService authService) :
            base(logger, dateTimeUtil)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthUserAsync([FromBody] LoginUserRequest request)
        {
            TokenResponse tokenReponse = await _authService.AuthUserAsync(request);
            return Ok(tokenReponse);
        }
    }
}
