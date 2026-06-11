using InstantMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using InstantMessenger.Application.Interfaces;

namespace InstantMessenger.Infrastructure.Repositories
{
    public sealed class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await appDbContext.Users.FindAsync(id);
        }
        public async Task AddUserAsync(User user)
        {
            appDbContext.Users.Add(user);
            await appDbContext.SaveChangesAsync();
        }
        
        public async Task<bool> IsUsernameAvailableAsync(string username)
        {
            return !appDbContext.Users.Any(u => u.Username == username);
        }
        
        public async Task<User?> GetUserByUsernameAsync(string? username)
        {
            return await appDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        
        public async Task SaveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }
        
        public async Task<List<User>> GetUserByNickQuery(string nickQuery)
        {
            return await appDbContext.Users.Where(u => u.Nick.Contains(nickQuery)).ToListAsync();
        }
        
        public async Task<User?> GetUserByRefreshToken(string refreshToken)
        {
            return await appDbContext.Users.Where(u => u.RefreshToken.Equals(refreshToken)).FirstOrDefaultAsync();
        }
    }
}
