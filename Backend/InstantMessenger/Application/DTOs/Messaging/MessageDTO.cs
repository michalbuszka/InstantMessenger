namespace InstantMessenger.Application.DTOs.Messaging;

public class MessageDTO
{
    public record Message(string senderId, string receiverId, string content);
}