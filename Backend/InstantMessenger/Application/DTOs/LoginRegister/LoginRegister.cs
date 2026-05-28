namespace InstantMessenger.Application.DTOs.LoginRegister
{
    public record RegisterRequest(string Username, string Nick, string Password);
    public record LoginRegisterResponse(int Status, string[] Messages, string Token, string RefreshToken);
    
    public record LoginRequest(string Username, string Password);
    public record Refresh(string Token, string RefreshToken);
}