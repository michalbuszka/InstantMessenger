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
        try
        {
            var id = Guid.Parse(User.Identity?.Name);
            if (await userService.UpdateUserDataAsync(id, userSettings))
                return Ok();
        }
        catch (Exception e)
        {

        }
        return BadRequest();
    }   
    [Authorize]
    [HttpGet("getUserData")]
    public async Task<IActionResult> GetUserData()
    {
        try
        {
            var id = Guid.Parse(User.Identity?.Name);
            var data = await userService.GetUserDataAsync(id);
            if (data == null)
                return BadRequest();
            return Ok(data);
        }
        catch (Exception e)
        {
            
        }
        return BadRequest();
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
        try
        {
            var Id = Guid.Parse(id);
            return Ok(await userService.GetUserById(Id));
        }
        catch (Exception e)
        {
            
        }

        return BadRequest();

    }  
}