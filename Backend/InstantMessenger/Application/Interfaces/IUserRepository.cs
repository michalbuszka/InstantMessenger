using InstantMessenger.Domain.Entities;

namespace InstantMessenger.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);
    Task AddUserAsync(User user);
    Task<bool> IsUsernameAvailableAsync(string username);
    Task<User?> GetUserByUsernameAsync(string? username);
    Task SaveChangesAsync();
    Task<List<User>> GetUserByNickQuery(string nickQuery);
    Task<User?> GetUserByRefreshToken(string refreshToken);
}