using System.ComponentModel.DataAnnotations;

namespace InstantMessenger.Domain.Entities
{
    public class User
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<ConversationUser> ConversationUsers { get; set; } = new();

        public User()
        {
            
        }

        public User(string username, string password)
        {
            Username = username;
            PasswordHash = password;
        }
    }
}