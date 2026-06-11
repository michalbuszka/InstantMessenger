using InstantMessenger.Application.DTOs.User.Messaging;
using InstantMessenger.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Infrastructure;

public class MessageNotifier : IMessageNotifier
{
    public async Task NotifyUsers(MessageDto message, List<string> userIds, IHubCallerClients clients, Guid conversationId)
    {
        if (userIds.Count == 0) return;
        await clients.Users(userIds).SendAsync(
            "ReceiveMessage", 
            message.SenderId,
            message.Nick, 
            message.Content, 
            message.Date, 
            conversationId
        );
    }
}