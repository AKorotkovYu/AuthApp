using System;
using OneChat.DAL.Entities;
using OneChat.DAL.Interfaces;
using OneChat.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace OneChat.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private OperatorContext db;
        private UserRepository userRepository;
        private ChatMessageRepository chatMessageRepository;
        private ChatRepository chatRepository;

        public EFUnitOfWork(DbContextOptions<OperatorContext> options)
        {
            db = new OperatorContext(options);
        }
        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public IRepository<Chat> Chats
        {
            get
            {
                if (chatRepository == null)
                    chatRepository = new ChatRepository(db);
                return chatRepository;
            }
        }

        public IRepository<ChatMessage> ChatMessages
        {
            get
            {
                if (chatMessageRepository == null)
                    chatMessageRepository = new ChatMessageRepository(db);
                return chatMessageRepository;
            }
        }

        public async Task Save()
        {
           await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
