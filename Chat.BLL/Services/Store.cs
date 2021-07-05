using System.Collections.Generic;
using System.Linq;
using OneChat.DAL.EF;
using OneChat.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;
//using OneChat.BLL.BusinessModels;
using OneChat.DAL.Interfaces;
using OneChat.BLL.Infrastructure;
using AutoMapper;
using System;


namespace OneChat.BLL.Interfaces
{
    public class Store : IStore
    {
        IUnitOfWork Database { get; set; }

        public Store(IUnitOfWork uow)
        {
            Database = uow;
        }

        public UserDTO GetUser(string email, string password)
        {
            User user = Database.Users.Find(u => u.Email == email && u.Password == password).First();
            if (user == null)
                return null;
            else 
                return new UserDTO { Chats = user.Chats, DateOfRegistration = user.DateOfRegistration, Email = user.Email, Id = user.Id, Nickname = user.Nickname };
        }

        public UserDTO GetUser(string nickname)
        {
            User user = Database.Users.Find(u => u.Nickname==nickname).First();
            if (user == null)
                return null;
            else
                return new UserDTO { Chats = user.Chats, DateOfRegistration = user.DateOfRegistration, Email = user.Email, Id = user.Id, Nickname = user.Nickname };
        }

        public UserDTO AddNewUser(UserDTO userDTO)
        {
            User user = new User { Nickname = userDTO.Nickname, Chats = userDTO.Chats, DateOfRegistration = DateTime.Now, Email = userDTO.Email, Password = userDTO.Password };
            Database.Users.Create(user);
            return new UserDTO { Id = user.Id, Chats = user.Chats, DateOfRegistration = user.DateOfRegistration, Email = user.Email, Nickname = user.Nickname};
        }

        public void AddUserToChat(int userId, int chatId)
        {
            User user = Database.Users.Get(userId);
            Chat chat = Database.Chats.Get(chatId);

            chat.ChatUsers.Add(user);
            user.Chats.Add(chat);

            /*SaveMessage(new ChatMessageDTO
            {    
                Id = user.Id,
                isOld = true,
                ChatId = chat.Id,
                ChatName = chat.ChatName,
                Nickname = $"/ {user.Id}",
                Message = "+",
                TimeOfPosting = DateTime.Now,
            });*/

            Database.Chats.Get(chatId).ChatUsers.Add(user);
            Database.Users.Get(userId).Chats.Add(chat);
        }

        public void DelUserFromChat(int userId, int chatId)
        { 
            User user = Database.Users.GetAll().Where(c => c.Id == userId).First();
            Chat chat = Database.Chats.GetAll().Where(c => c.Id == chatId).First();
            chat.ChatUsers.Remove(user);
            user.Chats.Remove(chat);

            /*SaveMessage(new ChatMessageDTO
            {
                Id = user.Id,
                isOld = true,
                ChatId = chat.Id,
                ChatName = chat.ChatName,
                Nickname = $"/ {user.Id}",
                Message = "-",
                TimeOfPosting = DateTime.Now,
            });*/
        }

        public Dictionary<UserDTO, int> AllAnotherUsers(int chatId)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            
            Dictionary<User, int> users = new();

            Chat chat = Database.Chats.GetAll().Where(c => c.Id == chatId).First();

            foreach (User user in Database.Users.GetAll())
            {
                if (user.Chats.Find(c => c.Id == chat.Id) == null)
                    users.Add(user, chatId);
            }
            return mapper.Map<Dictionary<User, int>, Dictionary<UserDTO, int>>(users);
        }

        public List<ChatDTO> GetAllUserChats(int userId)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Chat, ChatDTO>()).CreateMapper();
            return mapper.Map<List<Chat>, List<ChatDTO>>(Database.Users.GetAll().Where(c => c.Id == userId).First().Chats);
        }

        public void SaveMessage(ChatMessageDTO messageDTO)
        {
            ChatMessage message = new() { 
                Id = messageDTO.Id, 
                IsOld = messageDTO.isOld, 
                ChatId = messageDTO.ChatId, 
                Message = messageDTO.Message, 
                Nickname = messageDTO.Nickname, 
                ChatName = messageDTO.ChatName,  
                TimeOfPosting = messageDTO.TimeOfPosting 
            };
            Database.ChatMessages.Create(message);
        }

        public void RemoveMessage(int messageId)
        {
            Database.ChatMessages.Delete(messageId);
        }

        public ChatDTO AddChat(ChatDTO chatDTO)
        {
            Chat chat = new(){ 
                AdminId=chatDTO.AdminId, 
                ChatName=chatDTO.ChatName, 
                ChatUsers=chatDTO.ChatUsers,
                ChatMessages=chatDTO.ChatMessages
            };

            Database.Chats.Create(chat);
            SaveChanges();

            return new ChatDTO {
                Id = chat.Id,
                AdminId = chat.AdminId, 
                ChatName = chat.ChatName, 
                ChatUsers = chat.ChatUsers, 
                ChatMessages = chat.ChatMessages, 
                };
        }

        public void RemoveChat(int chatId)
        {
            Chat chat = Database.Chats.Get(chatId);
            foreach(User user in Database.Users.GetAll())
            {
                user.Chats.Remove(chat);
            }
            Database.Chats.Delete(chatId);
        }

        public ChatMessageDTO GetMessage(int mesId)
        {
            ChatMessage chatMessage = Database.ChatMessages.GetAll().FirstOrDefault(x => x.Id == mesId);
            return new()
            {
                Id = chatMessage.Id,
                isOld = chatMessage.IsOld,
                ChatId = chatMessage.ChatId,
                Message = chatMessage.Message,
                ChatName = chatMessage.ChatName,
                Nickname = chatMessage.Nickname,
                TimeOfPosting = chatMessage.TimeOfPosting
            };
        }

        public List<ChatMessageDTO> GetMessages(int chatId)
        {
            List<ChatMessage> chatMessages= Database.ChatMessages.GetAll().Where(c=>c.ChatId==chatId).ToList();
            List<ChatMessageDTO> chatMessageDTOs = new(); 
            
            foreach(ChatMessage chatMessage in chatMessages)
            {
                chatMessageDTOs.Add(new ChatMessageDTO
                {
                    ChatId = chatMessage.ChatId,
                    ChatName = chatMessage.ChatName,
                    Id = chatMessage.Id,
                    Message = chatMessage.Message,
                    isOld = chatMessage.IsOld,
                    Nickname = chatMessage.Nickname,
                    TimeOfPosting = chatMessage.TimeOfPosting
                });
            }
            return chatMessageDTOs;
        }

        public UserDTO GetUser(int userId)
        {
            User user = Database.Users.Get(userId);
            return new UserDTO {
                Id=user.Id,
                Email=user.Email, 
                Nickname=user.Nickname,
                Password=user.Password,
                DateOfRegistration=user.DateOfRegistration
            };
        }

        public ChatDTO GetChat(int chatId)
        {
            Chat chat = Database.Chats.Get(chatId);
            return new ChatDTO { AdminId = chat.AdminId, ChatName = chat.ChatName, Id=chat.Id};
        }
        
        public void SaveChanges()
        {
            Database.Save();
        }
    }
}