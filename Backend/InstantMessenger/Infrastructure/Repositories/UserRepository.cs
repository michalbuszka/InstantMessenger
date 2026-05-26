using InstantMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessenger.Infrastructure.Repositories
{
    public sealed class UserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _appDbContext.Users.FindAsync(id);
        }
        public async Task AddUserAsync(User user)
        {
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
        }
        
        public async Task<bool> IsUsernameAvailableAsync(string username)
        {
            return !_appDbContext.Users.Any(u => u.Username == username);
        }
        
        public async Task<User?> GetUserByUsernameAsync(string? username)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        
        public async Task SaveUserAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
        
        public async Task<List<User>> GetUserByNickQuery(string nickQuery)
        {
            return await _appDbContext.Users.Where(u => u.Nick.Contains(nickQuery)).ToListAsync();
        }
        
        
    }
}
