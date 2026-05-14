using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InstantMessenger.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Username { get; set; } 
        public List<ConversationUser> ConversationUsers { get; set; } = new();
    }
}
