using InstantMessenger.Application.DTOs;
using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserService userService;
        public LoginController (UserService userService)
        {
            this.userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            await userService.AddUserAsync(registerRequestDTO.Username, registerRequestDTO.Password);
            return Ok();
        }
    }
}
