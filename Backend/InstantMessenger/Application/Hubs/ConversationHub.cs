using InstantMessenger.Application.Services;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace InstantMessenger.Application.Hubs;

[Authorize]
public class ConversationHub(MessagingService messagingService, UserRepository userRepository) : Hub
{
    public async Task SendMessage(string userId, string messageContent)
    {
        var senderLogin = Context.UserIdentifier;
        var user = await userRepository.GetUserByUsernameAsync(senderLogin);
        var targetUser = await userRepository.GetUserByIdAsync(Guid.Parse(userId));
        if (user is null || targetUser is null)
            return;
        await messagingService.SendMessage(user.Id, targetUser.Id, messageContent, Clients);
    }
}