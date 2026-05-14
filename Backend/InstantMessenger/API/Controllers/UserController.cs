using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.API.Controllers
{
    [ApiController]
    [Route($"api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        public UserController (UserService userService)
        {
            this.userService = userService;
        }
        [HttpGet("{nick}")]
        public async Task<IActionResult> AddUser(string username)
        {
            await userService.AddUserAsync(username);
            return Ok();
        }
    }
}
