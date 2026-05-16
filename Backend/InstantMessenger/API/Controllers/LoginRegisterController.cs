using InstantMessenger.Application.DTOs.LoginRegister;
using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Mvc;
using RegisterRequest = InstantMessenger.Application.DTOs.LoginRegister.RegisterRequest;

namespace InstantMessenger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginRegisterController : ControllerBase
    {
        private readonly UserService _userService;
        public LoginRegisterController (UserService userService)
        {
            this._userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody]  RegisterRequest registerLogin)
        {
            var response = await _userService.AddUserAsync(registerLogin);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody]  LoginRequest loginRequest)
        {
            var response = await _userService.LoginUserAsync(loginRequest);
            return Ok(response);
        }
    }
}
