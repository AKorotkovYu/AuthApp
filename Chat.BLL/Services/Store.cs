using System.Collections.Generic;
using System.Linq;
using OneChat.BLL.Interfaces;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;
using OneChat.DAL.Interfaces;
using AutoMapper;
using System;
using System.Threading.Tasks;


namespace OneChat.BLL.Services
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
            User user = Database.Users.GetAll().FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
                return null;
            else 
                return new UserDTO { Chats = user.Chats, DateOfRegistration = user.DateOfRegistration, Email = user.Email, Id = user.Id, Nickname = user.Nickname };
        }

        public UserDTO GetUser(string nickname)
        {
            User user = Database.Users.GetAll().FirstOrDefault(u => u.Nickname == nickname);
            if (user == null)
                return null;
            else
                return new UserDTO { Chats = user.Chats, DateOfRegistration = user.DateOfRegistration, Email = user.Email, Id = user.Id, Nickname = user.Nickname };
        }

        public async Task<UserDTO> AddNewUser(UserDTO userDTO)
        {
            User user = new() { Nickname = userDTO.Nickname, Chats = userDTO.Chats, DateOfRegistration = DateTime.Now, Email = userDTO.Email, Password = userDTO.Password };
            Database.Users.Create(user);
            await Database.Save();
            return new UserDTO { Id = user.Id, Chats = user.Chats, DateOfRegistration = user.DateOfRegistration, Email = user.Email, Nickname = user.Nickname};
        }

        public async Task AddUserToChat(int userId, int chatId)
        {
            User user = Database.Users.Get(userId);
            Chat chat = Database.Chats.Get(chatId);

            chat.ChatUsers.Add(user);
            user.Chats.Add(chat);

            Database.Chats.Get(chatId).ChatUsers.Add(user);
            Database.Users.Get(userId).Chats.Add(chat);
            await Database.Save();
        }

        public async Task DelUserFromChat(int userId, int chatId)
        { 
            User user = Database.Users.GetAll().Where(c => c.Id == userId).First();
            Chat chat = Database.Chats.GetAll().Where(c => c.Id == chatId).First();
            chat.ChatUsers.Remove(user);
            if(chat.AdminId==user.Id)
            {
                chat.AdminId = chat.ChatUsers.First().Id;
            }
            user.Chats.Remove(chat);
            await Database.Save();
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
            List<ChatDTO> chatsDTO = new();
            
            foreach(var chat in Database.Users.Get(userId).Chats)
            {
                Chat fullChat=Database.Chats.Get(chat.Id);
                chatsDTO.Add(new()
                {
                    ChatMessages = chat.ChatMessages,
                    ChatName = chat.ChatName,
                    ChatUsers = fullChat.ChatUsers,
                    AdminId = chat.AdminId,
                    Id = chat.Id
                });
            }
            return chatsDTO;
        }

        public async Task SaveMessage(ChatMessageDTO messageDTO)
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
            await Database.Save();
        }

        public async Task RemoveMessage(int messageId)
        {
            Database.ChatMessages.Delete(messageId);
           await Database.Save();
        }

        public async Task<ChatDTO> AddChat(ChatDTO chatDTO)
        {
            Chat chat = new(){ 
                AdminId=chatDTO.AdminId, 
                ChatName=chatDTO.ChatName, 
                ChatUsers=chatDTO.ChatUsers,
                ChatMessages=chatDTO.ChatMessages
            };

            Database.Chats.Create(chat);
            await Database.Save();

             return new ChatDTO {
                Id = chat.Id,
                AdminId = chat.AdminId, 
                ChatName = chat.ChatName, 
                ChatUsers = chat.ChatUsers, 
                ChatMessages = chat.ChatMessages, 
                };
        }

        public async Task RemoveChat(int chatId)
        {
            Chat chat = Database.Chats.Get(chatId);
            foreach(User user in Database.Users.GetAll())
            {
                user.Chats.Remove(chat);
            }
            Database.Chats.Delete(chatId);
            await Database.Save();
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

        private async Task<List<ChatMessage>> GetMessagesCheckOld(int chatId)
        {
            foreach (ChatMessage chatMessage in Database.ChatMessages.GetAll().Where(c => c.ChatId == chatId).ToList())
            {
                if (DateTime.Now >= chatMessage.TimeOfPosting.AddDays(+1))
                    chatMessage.IsOld = true;
                else
                    chatMessage.IsOld = false;
            }
            await Database.Save();
            return Database.ChatMessages.GetAll().Where(c => c.ChatId == chatId).ToList();
        }

        public async Task<List<ChatMessageDTO>> GetMessages(int chatId)
        {
            List<ChatMessage> chatMessages =  await GetMessagesCheckOld(chatId);
            List<ChatMessageDTO> chatMessageDTOs = new();
            

            foreach (ChatMessage chatMessage in chatMessages)
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
    }
}