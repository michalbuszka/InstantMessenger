namespace InstantMessenger.Application.DTOs;

public class UserDTO
{
    public record UserSettingsDTO(
        string? Email,
        string? FirstName,
        string? LastName,
        string? Nick,
        string? Avatar);
}