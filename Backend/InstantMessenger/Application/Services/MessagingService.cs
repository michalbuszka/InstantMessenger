using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Application.DTOs.Messaging;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Application.Services;

public sealed class MessagingService(ConversationRepository conversationRepository, UserRepository userRepository)
{
    public async Task SendMessage(Guid senderUserId, Guid targetUserId, string msgContent, IHubCallerClients clients)
    {
        var sender = await userRepository.GetUserByIdAsync(senderUserId);
        var target = await userRepository.GetUserByIdAsync(targetUserId);
        if (sender == null || target == null)
            return;
        var conversation = await conversationRepository.GetConversationAsync(sender, target) ?? await conversationRepository.AddPrivConversationAsync(sender, target);
        ConversationUser? senderCu = conversation.ConversationUsers.FirstOrDefault(u => u.User.Id == sender.Id);
        var message = new Message { SenderId = senderCu.Id, Content = msgContent, ConversationId = conversation.Id};
        await conversationRepository.AddMessage(conversation, message);
        List<Task> sendMsg = new List<Task>();
        foreach (var user in conversation.ConversationUsers)
        {
            sendMsg.Add(clients.User(user.User.Username).SendAsync("ReceiveMessage", senderUserId.ToString(), msgContent));
        }
        await Task.WhenAll(sendMsg);
    }
}