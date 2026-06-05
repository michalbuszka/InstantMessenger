using InstantMessenger.Application.DTOs.User.Messaging;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Application.Services;

public sealed class MessagingService(ConversationRepository conversationRepository, UserRepository userRepository)
{
    private async Task NotifyUsers(MessageDto message,Conversation conversation, IHubCallerClients clients)
    {
        var sendMsg = new List<Task>();
        foreach (var user in conversation.ConversationUsers)
            sendMsg.Add(clients.User(user.User.Username).SendAsync("ReceiveMessage", message.SenderId, message.Nick, message.Content, message.Date));
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
        var date = DateTimeOffset.UtcNow;
        var message = new Message { SenderId = senderCu.Id, Content = msgContent, ConversationId = conversation.Id, date = date};
        await conversationRepository.AddMessage(conversation, message);
        await NotifyUsers(new MessageDto(senderUserId.ToString(), message.Sender.Nick, message.Content, date.ToString()), conversation, clients);
    }

    public async Task<List<MessageDto>> GetMessages(string username, Guid userId)
    {
        var user1 = await userRepository.GetUserByUsernameAsync(username);
        var user2 = await userRepository.GetUserByIdAsync(userId);
        if (user1 == null || user2 == null)
        {
            throw new Exception("Error!");
        }
        var conversation = await conversationRepository.GetConversationAsync(user1, user2);
        if (conversation == null)
        {
            throw new Exception("Error!");
        }

        var messages = conversationRepository.GetLastNMessages(conversation, 10);
        return messages.Select(m => new MessageDto(m.Sender.User.Id.ToString(), m.Sender.Nick, m.Content, m.date.ToString())).ToList();

    }
}