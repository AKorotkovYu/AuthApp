using Microsoft.EntityFrameworkCore;
using OneChat.DAL.Entities;

namespace OneChat.DAL.EF
{
    public class BotOperatorContext : DbContext
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public BotOperatorContext(DbContextOptions<OperatorContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
