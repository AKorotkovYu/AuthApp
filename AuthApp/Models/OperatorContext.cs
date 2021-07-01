using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Models
{
    public class OperatorContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public OperatorContext(DbContextOptions<OperatorContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
