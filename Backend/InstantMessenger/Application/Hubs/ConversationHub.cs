using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace InstantMessenger.Application.Hubs;

public class ConversationHub(ILogger<ConversationHub> logger) : Hub
{
    public async Task SendMessage(string userId, string message)
    {
        logger.LogInformation(message);
        await Clients.All.SendAsync("ReceiveMessage", userId, message);
    }
}