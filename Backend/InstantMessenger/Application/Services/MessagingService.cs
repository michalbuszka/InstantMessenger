using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;

namespace InstantMessenger.Application.Services;

public sealed class MessagingService(ConversationRepository conversationRepository, UserRepository userRepository)
{
    public async Task SendMessage(Guid senderUserId, Guid targetUserId, string msgContent)
    {
        var sender = await  userRepository.GetUserByIdAsync(senderUserId);
        var target = await userRepository.GetUserByIdAsync(senderUserId);
        if (sender == null || target == null)
            return;
        var conversation = await conversationRepository.GetConversationAsync(sender, target) ?? await conversationRepository.AddPrivConversationAsync(sender, target);
        var message = new Message { SenderId = senderUserId, Content = msgContent };
        await conversationRepository.AddMessage(conversation, message);
    }
}