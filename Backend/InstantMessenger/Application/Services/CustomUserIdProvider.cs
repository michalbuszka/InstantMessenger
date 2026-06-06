using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Application.Services;

public sealed class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var id = connection.User?.FindFirst("unique_name")?.Value;
        if (string.IsNullOrEmpty(id))
            id = connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        return id;
    }
}