using InstantMessenger.Application.Interfaces;
using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace InstantMessenger.Application.Hubs;

[Authorize]
public class ConversationHub(MessagingService messagingService, IUserRepository userRepository) : Hub
{
    public async Task SendMessage(string userId, string messageContent)
    {
        try
        {
            await messagingService.SendMessage(Guid.Parse(Context.UserIdentifier), Guid.Parse(userId), messageContent, Clients);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}