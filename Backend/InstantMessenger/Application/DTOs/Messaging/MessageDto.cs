namespace InstantMessenger.Application.DTOs.User.Messaging;

public record MessageDto(Guid SenderId,  String Nick, String Content, String Date);