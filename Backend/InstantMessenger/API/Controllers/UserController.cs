using InstantMessenger.Application.DTOs;
using InstantMessenger.Application.Services;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(JwtService jwtService, UserService userService) : ControllerBase
{
    private readonly JwtService _jwtService = jwtService;

    [HttpPost("updateUserData")]
    public async Task<IActionResult> UpdateUserData([FromBody] UserDTO.UserSettingsDTO userSettings)
    {
        var username = User.Identity?.Name;
        if (await userService.UpdateUserDataAsync(username, userSettings))
            return Ok();
        return BadRequest();
    }   
    [HttpGet("getUserData")]
    public async Task<IActionResult> GetUserData()
    {
        var username = User.Identity?.Name;
        var data = await userService.GetUserDataAsync(username);
        if (data == null)
            return BadRequest();
        return Ok(data);
    }  
    
}