namespace InstantMessenger.Application.DTOs.User;

public record UserSettingsDto(
    string? Email,
    string? FirstName,
    string? LastName,
    string? Nick,
    string? Avatar);

public record ContactDto(string Id, string Nick, string Avatar);

public record EditUserNickDto(string id, string newNick);