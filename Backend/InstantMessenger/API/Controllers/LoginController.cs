using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Mvc;
using RegisterRequest = InstantMessenger.Application.DTOs.LoginRegister.RegisterRequest;

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
        public async Task<IActionResult> register([FromBody]  RegisterRequest registerLogin)
        {
            var response = await userService.AddUserAsync(registerLogin.Username, registerLogin.Password);
            return Ok(response);
        }
    }
}
