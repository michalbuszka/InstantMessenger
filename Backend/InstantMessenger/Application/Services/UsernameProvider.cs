using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Application.Services;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var username = connection.User?.FindFirst("unique_name")?.Value;
        if (string.IsNullOrEmpty(username))
        {
            username = connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
        return username;
    }
}