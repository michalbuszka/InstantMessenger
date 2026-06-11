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
    [HttpGet("messages/{conversatrionIdOrUserId}")]
    public async Task<IActionResult> GetMessages(Guid conversatrionIdOrUserId)
    {
        try
        {
            var myId = Guid.Parse(User.Identity?.Name);
            return Ok(await messagingService.GetMessages(myId, conversatrionIdOrUserId));
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
            var myId = Guid.Parse(User.Identity?.Name);
            var conversationId = Guid.Parse(id);
            return Ok(await messagingService.GetConversationContacts(myId, conversationId));
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
            await messagingService.EditConversationUserNickname(myId, editUserNickDto.id, editUserNickDto.newNick);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
    
}