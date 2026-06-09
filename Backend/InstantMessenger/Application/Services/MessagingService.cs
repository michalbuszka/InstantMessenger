using InstantMessenger.Application.DTOs.User;
using InstantMessenger.Application.DTOs.User.Messaging;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Application.Services;

public sealed class MessagingService(ConversationRepository conversationRepository, UserRepository userRepository)
{
    private async Task NotifyUsers(MessageDto message, Conversation conversation, IHubCallerClients clients)
    {
        var sendMsg = new List<Task>();
        foreach (var user in conversation.ConversationUsers)
            sendMsg.Add(clients.User(user.User.Id.ToString()).SendAsync("ReceiveMessage", message.SenderId,
                message.Nick, message.Content, message.Date, conversation.Id));
        await Task.WhenAll(sendMsg);
    }

    public async Task SendMessage(Guid senderUserId, Guid userOrConversationId, string msgContent, IHubCallerClients clients)
    {
        var conversation = await conversationRepository.GetConversationByIdAsync(userOrConversationId);
        if (conversation == null)
        {
            conversation = await conversationRepository.GetPrivConversationAsync(senderUserId, userOrConversationId);
            if (conversation == null)
            {
                var sender = await userRepository.GetUserByIdAsync(senderUserId);
                var target = await userRepository.GetUserByIdAsync(senderUserId);
                if (sender == null || target == null)
                    return;
                conversation = await conversationRepository.AddPrivConversationAsync(sender, target);
            }
        }
        await SendMessageInExistingConversation(senderUserId, conversation.Id, msgContent, clients);
    }

    private async Task SendMessageInExistingConversation(Guid senderUserId, Guid conversationid, string msgContent, IHubCallerClients clients)
    {
        var sender = await userRepository.GetUserByIdAsync(senderUserId);
        if (sender == null)
            return;
        var conversation = await conversationRepository.GetConversationByIdAsync(conversationid);
        if (conversation == null)
            return;
        var senderCu = conversation.ConversationUsers.FirstOrDefault(u => u.User.Id == sender.Id);
        if (senderCu == null)
            return;
        var date = DateTimeOffset.UtcNow;
        var message = new Message
            { SenderId = senderCu.Id, Content = msgContent, ConversationId = conversation.Id, date = date };
        await conversationRepository.AddMessage(conversation, message);
        await NotifyUsers(new MessageDto(senderUserId, senderCu.Nick, message.Content, date.ToString()), conversation,
            clients);
    }
    
    public async Task<List<MessageDto>> GetMessages(Guid myId, Guid conversationIdOrUserId)
    {
        var conversation = await conversationRepository.GetConversationByIdAsync(conversationIdOrUserId);
        if (conversation == null || !conversation.ConversationUsers.Any(cu => cu.User.Id == myId))
        {
            conversation = await conversationRepository.GetPrivConversationAsync(myId, conversationIdOrUserId);
        }
        if (conversation == null)
        {
            throw new Exception("Error!");
        }
        var messages = await conversationRepository.GetLastNMessages(conversation.Id, 10);
        return messages.Select(m => new MessageDto(m.Sender.User.Id, m.Sender.Nick, m.Content, m.date.ToString()))
            .ToList();
    }

    public async Task<List<ContactDto>> GetConversationContacts(Guid myId, Guid conversationOrUserId)
    {
        var conversation = await conversationRepository.GetConversationByIdAsync(conversationOrUserId);
        if (conversation == null)
        {
            conversation = await conversationRepository.GetPrivConversationAsync(myId, conversationOrUserId);
        }
        if (conversation == null || !conversation.ConversationUsers.Any(cu => cu.User.Id == myId))
            throw new Exception("Error!");
        return conversation.ConversationUsers.Select(u => new ContactDto(u.Id, u.Nick, u.User.Avatar!)).ToList();
    }

    public async Task<bool> EditConversationUserNickname(Guid myId, Guid conversationUserId, string newNick)
    {
        var conversation =
            await conversationRepository.GetConversationFromConversationUserAndUserAsync(myId, conversationUserId);
        if (conversation == null)
            return false;
        var cu = conversation.ConversationUsers.FirstOrDefault(cu => cu.Id == conversationUserId);
        if (cu == null)
            return false;
        cu.Nick = newNick;
        await userRepository.SaveChangesAsync();
        return true;
    }
}