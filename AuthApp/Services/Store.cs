using System.Collections.Generic;
using System.Linq;
using AuthApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace AuthApp.Services
{
    public class Store : IStore
    {
        private OperatorContext db;

        public async Task AddUserToChat(int userId, int chatId)
        {
            User user = db.Users.Where(us => us.Id == userId).Single();
            Chat chat = db.Chats.Where(us => us.Id == chatId).Single();

            chat.ChatUsers.Add(user);
            user.Chats.Add(chat);

            db.Chats.Where(c => c == chat).First().ChatUsers.Add(user);
            db.Users.Where(c => c == user).First().Chats.Add(chat);
            await SaveChanges();
        }

        public async Task DelUserFromChat(int userId, int chatId)
        {
            User user = db.Users.Include(c=>c.Chats).Where(us => us.Id == userId).Single();
            Chat chat = db.Chats.Include(c => c.ChatUsers).Where(us => us.Id == chatId).Single();

            chat.ChatUsers.Remove(user);
            user.Chats.Remove(chat);
            await SaveChanges();
        }

        public void SetDb(OperatorContext db)
        {
            this.db = db;
        }

        public Dictionary<User, int> AllAnotherUsers(int chatId)
        {
            Dictionary<User, int> users = new();
            Chat chat = db.Chats.Include(c => c.ChatUsers).Where(c => c.Id == chatId).First();

            foreach (User user in db.Users.Include(c => c.Chats))
            {
                if (user.Chats.Find(c => c.Id == chat.Id) == null)
                    users.Add(user, chatId);
            }
            return users;
        }

        public List<Chat> GetAllUserChats(string userName)
        {
            List<Chat> chats = new();

            foreach(Chat chat in db.Chats.Include(c=>c.ChatUsers))
            {
                foreach(var user in chat.ChatUsers)
                    if(user.Nickname==userName)
                        chats.Add(chat);
            }
            return chats;
        }

        public void SaveMessage(ChatMessage message)
        {
            db.ChatMessages.Add(message);
        }

        public void Remove(ChatMessage message)
        {
            db.ChatMessages.Remove(message);
        }

        public void Remove(Chat chat)
        {
            foreach(User user in db.Users)
            {
                user.Chats.Remove(chat);
            }
            db.Chats.Remove(chat);
        }

        public ChatMessage FindMessage(int mesId)
        {
            return db.ChatMessages.FirstOrDefault(x => x.Id == mesId);
        }

        public List<ChatMessage> FindMessages(int chatId)
        {
            return db.ChatMessages.Where(c => c.ChatId == chatId).ToList();
        }


        public User FindUser(string userName)
        {
           return db.Users.Where(us => us.Nickname == userName).Single();
        }

        public async Task<Chat> FindChat(int chatId)
        {
            return await db.Chats.FirstOrDefaultAsync(u => u.Id == chatId);
        }

        public void Add(Chat chat)
        {
            db.Chats.Add(chat);
        }
        
        public  async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }
    }
}