namespace InstantMessenger.Application.DTOs.Messaging;

public class MessageDTO
{
    public record MessageDto(string SenderId, string Content);
}