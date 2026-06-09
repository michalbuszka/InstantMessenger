namespace InstantMessenger.Application.DTOs.User;

public record UserSettingsDto(
    string? Email,
    string? FirstName,
    string? LastName,
    string? Nick,
    string? Avatar);

public record ContactDto(Guid Id, string Nick, string Avatar);

public record EditUserNickDto(Guid id, string newNick);