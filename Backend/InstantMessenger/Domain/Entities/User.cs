using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InstantMessenger.Domain.Entities
{
    public class User
    {
        [Key]
        public int? Id { get; set; } 
        public string? Nick { get; set; }
    }
}
