using System;
using System.Collections.Generic;
using System.Linq;
using OneChat.DAL.Entities;
using OneChat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using OneChat.DAL.EF;

namespace OneChat.DAL.Repositories
{
    class UserRepository : IRepository<User>
    {
        private readonly OperatorContext db;

        public UserRepository(OperatorContext context)
        {
            this.db = context;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users
                .Include(c=>c.Chats);
        }

        public User Get(int id)
        {
            return db.Users
                .Include(c=>c.Chats)
                .Where(c=>c.Id==id)
                .First();
        }

        public void Create(User user)
        {
            db.Users.Add(user);
        }

        public void Update(User User)
        {
            db.Entry(User).State = EntityState.Modified;
        }

        public IEnumerable<User> Find(Func<User, Boolean> predicate)
        {
            return db.Users
                .Include(c=>c.Chats)
                .Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            User User = db.Users.Find(id);
            if (User != null)
                db.Users.Remove(User);
        }
    }
}
