using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;

namespace InstantMessenger.Application.Services;

public sealed class MessagingService(ConversationRepository conversationRepository, UserRepository userRepository)
{
    public async Task SendMessage(Guid senderUserId, Guid targetUserId, string msgContent)
    {
        var sender = await  userRepository.GetUserByIdAsync(senderUserId);
        var target = await userRepository.GetUserByIdAsync(targetUserId);
        if (sender == null || target == null)
            return;
        var conversation = await conversationRepository.GetConversationAsync(sender, target) ?? await conversationRepository.AddPrivConversationAsync(sender, target);
        ConversationUser? senderCu = conversation.ConversationUsers.FirstOrDefault(u => u.User.Id == sender.Id);
        if (senderCu == null) 
            return;
        var message = new Message { SenderId = senderCu.Id, Content = msgContent, ConversationId = conversation.Id};
        await conversationRepository.AddMessage(conversation, message);
    }
}