using InstantMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Infrastructure.Repositories;

public sealed class ConversationRepository(AppDbContext appDbContext)
{
    public async Task<Conversation?> GetConversationAsync(User sender, User target)
    {
        return await appDbContext.Conversations
            .Where(c => !c.IsGroup)
            .Where(c => c.ConversationUsers.Any(cu => cu.User.Id == sender.Id))
            .Where(c => c.ConversationUsers.Any(cu => cu.User.Id == target.Id))
            .Include(c => c.ConversationUsers)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync();
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

    public List<Message> GetLastNMessages(Conversation conversation, int n)
    {
        return conversation.Messages
            .OrderByDescending(m => m.date) 
            .Take(n)                        
            .ToList();
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