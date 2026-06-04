using System.ComponentModel.DataAnnotations;

namespace InstantMessenger.Domain.Entities
{
    public class Message
    {
        [Key] 
        public int Id { get; set; }
        public required Guid SenderId { get; set; }
        public required int ConversationId { get; set; }
        public required string Content { get; set; }
        public required DateTimeOffset date { get; set; }
        public ConversationUser Sender { get; set; }
        public Conversation Conversation { get; set; }
        public List<Reaction> Reactions { get; set; } = new();
    }
}
