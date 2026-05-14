using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InstantMessenger.Domain.Entities
{
    public class Message
    {
        [Key] 
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public int ConversationId { get; set; }
        public string Content { get; set; } 
        public ConversationUser Sender { get; set; }
        public Conversation Conversation { get; set; }
        public List<Reaction> Reactions { get; set; } = new();
    }
}
