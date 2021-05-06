using EMS.Auth.API.Enums;
using EMS.Auth.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUsersService _usersService;

        public UserController(IUsersService usersService)
        {
            _usersService = usersService;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
