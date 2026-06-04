using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Application.Services;

public sealed class MessagingService(ConversationRepository conversationRepository, UserRepository userRepository)
{
    private async Task NotifyUsers(Message message, IHubCallerClients clients)
    {
        var sendMsg = new List<Task>();
        foreach (var user in message.Conversation.ConversationUsers)
            sendMsg.Add(clients.User(user.User.Username).SendAsync("ReceiveMessage", message.SenderId.ToString(), message.Content));
        await Task.WhenAll(sendMsg);
    }
    
    public async Task SendMessage(Guid senderUserId, Guid targetUserId, string msgContent, IHubCallerClients clients)
    {
        var sender = await userRepository.GetUserByIdAsync(senderUserId);
        var target = await userRepository.GetUserByIdAsync(targetUserId);
        if (sender == null || target == null)
            return;
        var conversation = await conversationRepository.GetConversationAsync(sender, target) ?? await conversationRepository.AddPrivConversationAsync(sender, target);
        var senderCu = conversation.ConversationUsers.FirstOrDefault(u => u.User.Id == sender.Id);
        if (senderCu == null)
            return;
        var message = new Message { SenderId = senderCu.Id, Content = msgContent, ConversationId = conversation.Id};
        await conversationRepository.AddMessage(conversation, message);
        await NotifyUsers(message, clients);
    }
}