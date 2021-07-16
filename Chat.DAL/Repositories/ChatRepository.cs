using System;
using System.Collections.Generic;
using System.Linq;
using OneChat.DAL.Interfaces;
using OneChat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using OneChat.DAL.EF;
using System.Threading.Tasks;

namespace OneChat.DAL.Repositories
{
    class ChatRepository : IRepository<Chat>
    {
        private OperatorContext db;

        public ChatRepository(OperatorContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Chat>> GetAllAsync()
        {
          return await db.Chats.Include(c=>c.ChatUsers).ToListAsync();
        }

        public async Task<Chat> GetAsync(int id)
        {
            var chats = db.Chats.Include(c => c.ChatUsers).Where(c => c.Id == id);
            return await chats.FirstAsync();
        }

        public IEnumerable<Chat> GetAll()
        {
            return db.Chats.Include(c => c.ChatUsers).ToList();
        }

        public Chat Get(int id)
        {
            var chats = db.Chats.Include(c => c.ChatUsers).Where(c => c.Id == id);
            return chats.First();
        }

        public void Create(Chat chat)
        {
            db.Chats.Add(chat);
        }

        public void Update(Chat chat)
        {
            db.Entry(chat).State = EntityState.Modified;
        }

        public IEnumerable<Chat> Find(Func<Chat, Boolean> predicate)
        {
            return db.Chats
                .Where(predicate)
                .ToList();
        }

        public void Delete(int id)
        {
            Chat chat = db.Chats
                .Find(id);
            if (chat != null)
                db.Chats
                    .Remove(chat);
        }
    }
}
