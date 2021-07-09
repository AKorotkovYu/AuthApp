using Microsoft.EntityFrameworkCore;
using OneChat.DAL.Entities;

namespace OneChat.DAL.EF
{
    public class OperatorContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatMessageFIFO> ChatMessagesFIFO { get; set; }

        public OperatorContext(DbContextOptions<OperatorContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
