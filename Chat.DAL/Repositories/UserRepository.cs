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
    class UserRepository : IRepository<User>
    {
        private readonly OperatorContext db;

        public UserRepository(OperatorContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await db.Users
                .Include(c=>c.Chats).ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            var users = db.Users.Include(c => c.Chats).Where(c => c.Id == id);
             return await users.FirstAsync();
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users
                .Include(c => c.Chats).ToList();
        }

        public User Get(int id)
        {
            var users = db.Users.Include(c => c.Chats).Where(c => c.Id == id);
            return users.First();
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
