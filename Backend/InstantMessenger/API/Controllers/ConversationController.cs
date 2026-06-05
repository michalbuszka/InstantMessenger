using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InstantMessenger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationController(MessagingService messagingService) : ControllerBase
{
    [Authorize]
    [HttpGet("messages/{id}")]
    public async Task<IActionResult> GetMessages(string id)
    {
        var username = User.Identity?.Name;
        if (username==null)
            return BadRequest();
        try
        {
            return Ok(await messagingService.GetMessages(username, Guid.Parse(id)));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}