using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessenger.Domain.Entities
{
    public class Conversations
    {
        public int? Id { get; set; }
        public int? UserId1 { get; set; }
        public int? Userid2 { get; set; }
    }
}
