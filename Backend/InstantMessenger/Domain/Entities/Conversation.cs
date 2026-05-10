using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InstantMessenger.Domain.Entities
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        public List<Message> Messages { get; set; } = new();
        public List<ConversationUser> ConversationUsers { get; set; } = new();
    }
}
