﻿using System;
using System.Collections.Generic;
using System.Linq;
using OneChat.DAL.Entities;
using OneChat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using OneChat.DAL.EF;
using System.Threading.Tasks;


namespace OneChat.DAL.Repositories
{
    class ChatMessageFIFORepository : IRepository<ChatMessageFIFO>
    {

        private OperatorContext db;

        public ChatMessageFIFORepository(OperatorContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<ChatMessageFIFO>> GetAllAsync()
        {
           return await db.ChatMessagesFIFO.ToListAsync();
        }

        public async Task<ChatMessageFIFO> GetAsync(int id)
        {
            return await db.ChatMessagesFIFO.FindAsync(id);
        }

        public IEnumerable<ChatMessageFIFO> GetAll()
        {
            return db.ChatMessagesFIFO.ToList();
        }

        public ChatMessageFIFO Get(int id)
        {
            return db.ChatMessagesFIFO.Find(id);
        }

        public void Create(ChatMessageFIFO message)
        {
            db.ChatMessagesFIFO.Add(message);
        }

        public void Update(ChatMessageFIFO message)
        {
            db.Entry(message).State = EntityState.Modified;
        }

        public IEnumerable<ChatMessageFIFO> Find(Func<ChatMessageFIFO, Boolean> predicate)
        {
            return db.ChatMessagesFIFO.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            ChatMessageFIFO message = db.ChatMessagesFIFO.Find(id);
            if (message != null)
                db.ChatMessagesFIFO.Remove(message);
        }
    }
}
