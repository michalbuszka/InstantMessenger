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
        try
        {
            var id1 = Guid.Parse(User.Identity?.Name);
            var id2 = Guid.Parse(id);
            return Ok(await messagingService.GetMessages(id1, id2));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}