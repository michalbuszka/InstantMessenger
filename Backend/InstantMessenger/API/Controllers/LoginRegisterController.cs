using InstantMessenger.Application.DTOs.LoginRegister;
using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Http;
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
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,   
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) 
            };
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
            return Ok(response.LoginRegisterResponse);
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody]  LoginRequest loginRequest)
        {
            var response = await _userService.LoginUserAsync(loginRequest);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,   
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) 
            };
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
            return Ok(response.LoginRegisterResponse);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return BadRequest("Brak tokenu w ciasteczku.");
            }
            var tokens = await _userService.RefreshUserAsync(refreshToken);
            if (tokens == null)
            {
                return Unauthorized("Nieprawidłowy token odświeżania.");
            }
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,   
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) 
            };
            Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);
            return Ok(new {Token = tokens.Token});
        }
        
    }
}
