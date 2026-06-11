using InstantMessenger.Domain.Entities;

namespace InstantMessenger.Application.Interfaces;

public interface IConversationRepository
{
    Task<Conversation?> GetPrivConversationAsync(Guid senderId, Guid userId);
    Task<Conversation?> GetConversationByIdAsync(Guid id);
    Task<Conversation> AddPrivConversationAsync(User sender, User target);
    Task AddMessage(Conversation conversation, Message message);
    Task<List<Message>> GetLastNMessages(Guid conversationId, int n);
    Task<Conversation?> GetConversationFromConversationUserAndUserAsync(Guid userId, Guid conversationUserId);
}