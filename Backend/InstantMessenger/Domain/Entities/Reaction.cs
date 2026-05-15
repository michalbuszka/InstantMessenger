using System.ComponentModel.DataAnnotations;

namespace InstantMessenger.Domain.Entities
{
    public class Reaction
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public User Sender { get; set; }
        public string Emoji { get; set; }
    }
}
