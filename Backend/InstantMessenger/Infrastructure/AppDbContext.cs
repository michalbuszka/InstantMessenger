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
        public DbSet<Conversation> Conversations;
        public DbSet<Message> Messages;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            modelBuilder.Entity<User>().HasMany(u => u.ConversationUsers).WithOne(cu => cu.User).HasForeignKey(cu => cu.UserId);

            modelBuilder.Entity<Message>().HasKey(m => m.Id);

            modelBuilder.Entity<Message>().HasOne(m => m.Conversation).WithMany(c => c.Messages).HasForeignKey(m => m.ConversationId);
            modelBuilder.Entity<Message>().HasMany(m => m.Reactions).WithOne(r => r.Message).HasForeignKey(r => r.MessageId);

            modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId);

            modelBuilder.Entity<Conversation>().HasKey(c => c.Id);
            modelBuilder.Entity<Conversation>().HasMany(c => c.ConversationUsers).WithOne(cu => cu.Conversation).HasForeignKey(cu => cu.ConversationId);

            modelBuilder.Entity<ConversationUser>().HasKey(cu => cu.Id);

            modelBuilder.Entity<Reaction>().HasKey(r => r.Id);

        }
    }
}
