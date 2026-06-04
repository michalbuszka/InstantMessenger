using InstantMessenger.Application.DTOs.User;
using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService) : ControllerBase
{
    [Authorize]
    [HttpPost("updateUserData")]
    public async Task<IActionResult> UpdateUserData([FromBody] UserSettingsDto userSettings)
    {
        var username = User.Identity?.Name;
        if (await userService.UpdateUserDataAsync(username, userSettings))
            return Ok();
        return BadRequest();
    }   
    [Authorize]
    [HttpGet("getUserData")]
    public async Task<IActionResult> GetUserData()
    {
        var username = User.Identity?.Name;
        var data = await userService.GetUserDataAsync(username);
        if (data == null)
            return BadRequest();
        return Ok(data);
    }  
    [HttpGet("getUserContacts/{nickQuery?}")]
    public async Task<IActionResult> GetUserContacts(string? nickQuery)
    {
        return Ok(await userService.GetUsersByNickQuery(nickQuery));
    }  
    [AllowAnonymous]
    [HttpGet("getUser/{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        return Ok(await userService.GetUserById(id));
    }  
}