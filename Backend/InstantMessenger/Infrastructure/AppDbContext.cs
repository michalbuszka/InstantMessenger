using InstantMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace InstantMessenger.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        {
            
        }
        public DbSet<User> Users;
        public DbSet<Conversations> Conversations;
    }
}
