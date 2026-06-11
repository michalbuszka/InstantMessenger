using InstantMessenger.Application.DTOs.User;
using InstantMessenger.Application.DTOs.User.Messaging;
using InstantMessenger.Application.Interfaces;
using InstantMessenger.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Application.Services;

public sealed class MessagingService(IConversationRepository conversationRepository, IUserRepository userRepository, IMessageNotifier messageNotifier)
{

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
                    throw new Exception("User or target user has not been found!");
                conversation = await conversationRepository.AddPrivConversationAsync(sender, target);
            }
        }
        await SendMessageInExistingConversation(senderUserId, conversation.Id, msgContent, clients);
    }

    private async Task SendMessageInExistingConversation(Guid senderUserId, Guid conversationId, string msgContent, IHubCallerClients clients)
    {
        var sender = await userRepository.GetUserByIdAsync(senderUserId);
        if (sender == null)
            throw new Exception("User has not been found.");
        var conversation = await conversationRepository.GetConversationByIdAsync(conversationId);
        if (conversation == null)
            throw new Exception("Conversation has not been found.");
        var senderCu = conversation.ConversationUsers.FirstOrDefault(u => u.User.Id == sender.Id);
        if (senderCu == null)
            throw new Exception("Conversation user has not not been found.");
        var date = DateTimeOffset.UtcNow;
        var message = new Message
            { SenderId = senderCu.Id, Content = msgContent, ConversationId = conversation.Id, date = date };
        await conversationRepository.AddMessage(conversation, message);
        var userIds = conversation.ConversationUsers.Select(u => u.User.Id.ToString()).ToList();
        await messageNotifier.NotifyUsers(new MessageDto(senderUserId, senderCu.Nick, message.Content, date.ToString()), userIds,
            clients, conversationId);
    }

    private async Task<Conversation> GetConversationFromConversationIdOrUserId(Guid myId, Guid conversationIdOrUserId)
    {
        var conversation = await conversationRepository.GetConversationByIdAsync(conversationIdOrUserId);
        if (conversation == null || !conversation.ConversationUsers.Any(cu => cu.User.Id == myId))
            conversation = await conversationRepository.GetPrivConversationAsync(myId, conversationIdOrUserId);
        if (conversation == null)
            throw new Exception("Conversation not found or user is not in conversation.");
        return conversation;
    }
    
    
    public async Task<List<MessageDto>> GetMessages(Guid myId, Guid conversationIdOrUserId)
    {
        var conversation = await GetConversationFromConversationIdOrUserId(myId, conversationIdOrUserId);
        var messages = await conversationRepository.GetLastNMessages(conversation.Id, 10);
        return messages.Select(m => new MessageDto(m.Sender.User.Id, m.Sender.Nick, m.Content, m.date.ToString()))
            .ToList();
    }

    public async Task<List<ContactDto>> GetConversationContacts(Guid myId, Guid conversationOrUserId)
    {
        var conversation = await GetConversationFromConversationIdOrUserId(myId, conversationOrUserId);
        return conversation.ConversationUsers.Select(u => new ContactDto(u.Id, u.Nick, u.User.Avatar!)).ToList();
    }

    public async Task EditConversationUserNickname(Guid myId, Guid conversationUserId, string newNick)
    {
        var conversation =
            await conversationRepository.GetConversationFromConversationUserAndUserAsync(myId, conversationUserId);
        if (conversation == null)
            throw new Exception("Convesation has not been found!");
        var cu = conversation.ConversationUsers.FirstOrDefault(cu => cu.Id == conversationUserId);
        if (cu == null)
            throw new Exception("Conversation User has not been found!");
        cu.Nick = newNick;
        await userRepository.SaveChangesAsync();
    }
}