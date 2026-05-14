using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InstantMessenger.Domain.Entities
{
    public class ConversationUser
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Nick { get; set; }
        public User User { get; set; }
        public Conversation Conversation { get; set; }
        public int ConversationId { get; set; }
    }
}
