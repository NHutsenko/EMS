using System.Threading.Tasks;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.ResponseModels;
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


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserAsync([FromBody] User user)
        {
            BaseResponse result = await _usersService.AddAsync(user);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody] User user)
        {
            if(HttpContext.Items["User"].ToString() != user.Login)
            {
                return Forbid();
            }
            BaseResponse result = await _usersService.UpdateAsync(user);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] User user)
        {
            BaseResponse result = await _usersService.DeleteAsync(user);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUserById([FromQuery] long userId)
        {
            return Ok(_usersService.GetById(userId));
        }
    }
}
