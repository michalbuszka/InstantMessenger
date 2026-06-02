using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using InstantMessenger.Application.Services;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace InstantMessenger.Application.Hubs;

[Authorize]
public class ConversationHub(ILogger<ConversationHub> logger, MessagingService messagingService, UserRepository userRepository) : Hub
{
    public async Task SendMessage(string userId, string messageContent)
    {
        var senderLogin = Context.UserIdentifier;
        User? user = await userRepository.GetUserByUsernameAsync(senderLogin);
        User? targetUser = await userRepository.GetUserByIdAsync(Guid.Parse(userId));
        if (user is null || targetUser is null)
            return;
        await messagingService.SendMessage(user.Id, targetUser.Id, messageContent, Clients);
    }
}