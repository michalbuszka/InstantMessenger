using InstantMessenger.Application.Services;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace InstantMessenger.Application.Hubs;

[Authorize]
public class ConversationHub(MessagingService messagingService, UserRepository userRepository) : Hub
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