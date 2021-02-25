using EMS.Common.Protos;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly Teams.TeamsClient _teamsClient;

        public TeamsController(Teams.TeamsClient teamsClient)
        {
            _teamsClient = teamsClient;
        }

        [HttpPost]
        public IActionResult AddAsync([FromBody] TeamData teamData)
        {
            BaseResponse response = _teamsClient.AddAsync(teamData);
            return Ok(response);
        }
    }
}
