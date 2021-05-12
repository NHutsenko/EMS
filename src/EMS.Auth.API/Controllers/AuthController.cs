using System.Threading.Tasks;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AuthUserAsync([FromBody] LoginUserRequest request)
        {
            TokenData tokenReponse = await _authService.AuthUserAsync(request);
            return Ok(tokenReponse);
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenData tokenData)
        {
            TokenData newTokenData = await _authService.RefreshTokenAsync(tokenData);
            return Ok(newTokenData);
        }
    }
}
