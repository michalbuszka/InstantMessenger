using InstantMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Infrastructure.Repositories;

public sealed class ConversationRepository(AppDbContext appDbContext)
{
    public async Task<Conversation?> GetPrivConversationAsync(Guid senderId, Guid userId)
    {
        return await appDbContext.Conversations
            .Where(c => !c.IsGroup)
            .Where(c => c.ConversationUsers.Any(cu => cu.User.Id == senderId))
            .Where(c => c.ConversationUsers.Any(cu => cu.User.Id == userId))
            .Include(c => c.ConversationUsers)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Conversation?> GetConversationByIdAsync(Guid id)
    {
        return await appDbContext.Conversations.Include(c => c.ConversationUsers).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Conversation> AddPrivConversationAsync(User sender, User target)
    {
        var conversation = new Conversation();
        var senderCu = new ConversationUser
        {
            User = sender,
            Nick = sender.Nick,
            ConversationId = conversation.Id
        };
        var targetCu = new ConversationUser
        {
            User = target,
            Nick = target.Nick,
            ConversationId = conversation.Id
        };
        conversation.ConversationUsers.Add(senderCu);
        conversation.ConversationUsers.Add(targetCu);
        await appDbContext.Conversations.AddAsync(conversation);
        await appDbContext.SaveChangesAsync();
        return conversation;
    }

    public async Task AddMessage(Conversation conversation, Message message)
    {
        conversation.Messages.Add(message);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<Message>> GetLastNMessages(Guid conversationId, int n)
    {
        return await appDbContext.Messages
            .Include(m => m.Sender).ThenInclude(u => u.User)
            .Where(m => m.ConversationId == conversationId).OrderByDescending(m => m.date) 
            .Take(n)               
            .ToListAsync();
    }
    
    public async Task<Conversation?> GetConversationFromConversationUserAndUserAsync(Guid userId, Guid conversationUserId)
    {
        return await appDbContext.Conversations
            .Where(c => !c.IsGroup)
            .Where(c => c.ConversationUsers.Any(cu => cu.User.Id == userId))
            .Where(c => c.ConversationUsers.Any(cu => cu.Id == conversationUserId))
            .Include(c => c.ConversationUsers)
            .FirstOrDefaultAsync();
    }
    
}