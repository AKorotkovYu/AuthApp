using System;
using System.Collections.Generic;
using System.Linq;
using OneChat.DAL.Entities;
using OneChat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using OneChat.DAL.EF;
using System.Threading.Tasks;

namespace OneChat.DAL.Repositories
{
    public class ChatMessageRepository : IRepository<ChatMessage>
    {
        private OperatorContext db;

        public ChatMessageRepository(OperatorContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<ChatMessage>> GetAllAsync()
        {
            return await db.ChatMessages.ToListAsync();
        }

        public async Task<ChatMessage> GetAsync(int id)
        {
            return await db.ChatMessages.FindAsync(id);
        }

        public IEnumerable<ChatMessage> GetAll()
        {
            return db.ChatMessages.ToList();
        }

        public ChatMessage Get(int id)
        {
            return db.ChatMessages.Find(id);
        }

        public void Create(ChatMessage message)
        {
            db.ChatMessages.Add(message);
        }

        public void Update(ChatMessage message)
        {
            db.Entry(message).State = EntityState.Modified;
        }

        public IEnumerable<ChatMessage> Find(Func<ChatMessage, Boolean> predicate)
        {
            return db.ChatMessages.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            ChatMessage message = db.ChatMessages.Find(id);
            if (message != null)
                db.ChatMessages.Remove(message);
        }
    }
}
