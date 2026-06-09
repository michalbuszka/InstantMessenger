using System.ComponentModel.DataAnnotations;

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
        public Guid ConversationId { get; set; }
    }
}
