using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace InstantMessenger.Application.Hubs;

[Authorize]
public class ConversationHub(ILogger<ConversationHub> logger) : Hub
{
    public async Task SendMessage(string userId, string message)
    {
        await Clients.User(Context.UserIdentifier!).SendAsync("ReceiveMessage", userId, message);
    }
}