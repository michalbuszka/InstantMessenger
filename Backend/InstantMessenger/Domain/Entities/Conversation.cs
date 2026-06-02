using System.ComponentModel.DataAnnotations;

namespace InstantMessenger.Domain.Entities
{
    public class Conversation
    {
        [Key] public int Id { get; set; }
        public bool IsGroup;
        public List<Message> Messages { get; set; } = new();
        public List<ConversationUser> ConversationUsers { get; set; } = new();
    }
}