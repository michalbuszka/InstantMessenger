using System.ComponentModel.DataAnnotations;

namespace InstantMessenger.Domain.Entities
{
    public class User
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Nick { get; set; }
        public string? Avatar { get; set; }
        public string PasswordHash { get; set; }
        public string RefreshToken { get; set; }
        public List<ConversationUser> ConversationUsers { get; set; } = [];
    }
}