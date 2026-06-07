using InstantMessenger.Application.DTOs.User;
using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InstantMessenger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConversationController(MessagingService messagingService) : ControllerBase
{
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
    
    [HttpGet("getConversationUsers/{id}")]
    public async Task<IActionResult> GetConversationUsers(string id)
    {
        try
        {
            var id1 = Guid.Parse(User.Identity?.Name);
            var id2 = Guid.Parse(id);
            return Ok(await messagingService.GetConversationContacts(id1, id2));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
    [HttpPatch("editNick")]
    public async Task<IActionResult> EditNick([FromBody] EditUserNickDto editUserNickDto)
    {
        try
        {
            var myId = Guid.Parse(User.Identity?.Name);
            var editUserId = Guid.Parse(editUserNickDto.id);
            return Ok(await messagingService.EditConversationUserNickname(myId, editUserId, editUserNickDto.newNick));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
    
}