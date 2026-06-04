namespace InstantMessenger.Application.DTOs.LoginRegister
{
    public record RegisterRequest(string Username, string Nick, string Password);
    public record LoginRegisterResponse(int Status, string[] Messages, string Token);
    
    public record LoginRequest(string Username, string Password);
    public record Tokens(string? id, string? Token, string? RefreshToken);
    public record LoginRegisterResponseWithRefreshToken(LoginRegisterResponse LoginRegisterResponse, string? RefreshToken);
}