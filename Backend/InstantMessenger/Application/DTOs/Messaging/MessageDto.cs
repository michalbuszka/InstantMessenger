namespace InstantMessenger.Application.DTOs.User.Messaging;

public record MessageDto(String SenderId,  String Nick, String Content, String Date);