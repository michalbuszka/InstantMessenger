using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InstantMessenger.Domain.Entities
{
    public class Reaction
    {
        [Key]
        public int Id { get; set; }
        public required int MessageId { get; set; }
        public required Message Message { get; set; }
        public required User Sender { get; set; }
        public string Emoji { get; set; } = string.Empty;
    }
}
