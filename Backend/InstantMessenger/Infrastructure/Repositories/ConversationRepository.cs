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
            .FirstOrDefaultAsync();
    }

    public async Task<Conversation> AddPrivConversationAsync(User sender, User target)
    {
        Conversation conversation = new Conversation();
        var senderCU = new ConversationUser
        {
            User = sender,
            Nick = sender.Nick,
            ConversationId = conversation.Id
        };
        var targetCU = new ConversationUser
        {
            User = target,
            Nick = target.Nick,
            ConversationId = conversation.Id
        };
        conversation.ConversationUsers.Add(senderCU);
        conversation.ConversationUsers.Add(targetCU);
        await appDbContext.Conversations.AddAsync(conversation);
        await appDbContext.SaveChangesAsync();
        return conversation;
    }

    public async Task AddMessage(Conversation conversation, Message message)
    {
        conversation.Messages.Add(message);
        await appDbContext.SaveChangesAsync();
    }
}