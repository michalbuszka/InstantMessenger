namespace InstantMessenger.Application.DTOs.LoginRegister
{
    public record RegisterRequest(string Username, string Password);
    public record RegisterResponse(int status, string[] messages, string token);
}