using System;
using System.Collections.Generic;
using System.Linq;
using OneChat.DAL.Interfaces;
using OneChat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using OneChat.DAL.EF;

namespace OneChat.DAL.Repositories
{
    class ChatRepository : IRepository<Chat>
    {
        private OperatorContext db;

        public ChatRepository(OperatorContext context)
        {
            this.db = context;
        }

        public IEnumerable<Chat> GetAll()
        {
            return db.Chats.Include(c=>c.ChatUsers);
        }

        public Chat Get(int id)
        {
      
            return db.Chats.Include(c=>c.ChatUsers).Where(c=>c.Id==id).First();
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
            return db.Chats.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Chat chat = db.Chats.Find(id);
            if (chat != null)
                db.Chats.Remove(chat);
        }
    }
}
