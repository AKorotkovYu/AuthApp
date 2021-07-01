using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models;

namespace AuthApp.Services
{
    public interface IStore
    {
        
        public void SetDb(OperatorContext db);
        
        public Task AddUserToChat(int userId, int chatId);
        public Task DelUserFromChat(int userId, int chatId);

        public User FindUser(string userName);
        public Task<Chat> FindChat(int chatId);

        public Dictionary<User, int> AllAnotherUsers(int chatId);
        public List<Chat> GetAllUserChats(string UserName);
        
        public ChatMessage FindMessage(int mesId);
        public void SaveMessage(ChatMessage message);
        public void Remove(ChatMessage message);
        public List<ChatMessage> FindMessages(int chatId);

        public void Add(Chat chat);
        public void Remove(Chat chat);
        
        public Task SaveChanges();
    }
}
