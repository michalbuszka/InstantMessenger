using InstantMessenger.Application.DTOs.User.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Application.Interfaces;

public interface IMessageNotifier
{
    Task NotifyUsers(MessageDto message, List<string> userIds, IHubCallerClients clients,
        Guid conversationId);
}