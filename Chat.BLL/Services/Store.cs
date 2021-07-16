using System.Collections.Generic;
using System.Linq;
using OneChat.BLL.Interfaces;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;
using OneChat.DAL.Interfaces;
using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Threading;


namespace OneChat.BLL.Services
{
    public class Store : IStore
    {
        IUnitOfWork Database { get; set; }

        public Store(IUnitOfWork uow)
        {
            Database = uow;
        }



        public async Task<UserDTO> GetUserAsync(string email, string password)
        {
            var users = await Database.Users.GetAllAsync();
            User user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
                return null;
            else
                return new UserDTO
                {
                    Chats = user.Chats,
                    DateOfRegistration = user.DateOfRegistration,
                    Email = user.Email,
                    Id = user.Id,
                    Nickname = user.Nickname,
                    AverageMessageCountInDay = user.AverageMessageCountInDay,
                    MessageActivity = user.MessageActivity,
                    LastMessageTime=user.LastMessageTime
                };
        }

        public async Task<UserDTO> GetUserAsync(string nickname)
        {
            var users = await Database.Users.GetAllAsync();
            User user = users.FirstOrDefault(u => u.Nickname == nickname);

            if (user == null)
                return null;
            else
                return new UserDTO
                {
                    Chats = user.Chats,
                    DateOfRegistration = user.DateOfRegistration,
                    Email = user.Email,
                    Id = user.Id,
                    Nickname = user.Nickname,
                    MessageActivity = user.MessageActivity,
                    AverageMessageCountInDay=user.AverageMessageCountInDay,
                };
        }
        //
        public async Task<UserDTO> AddNewUserAsync(UserDTO userDTO)
        {
            User user = new()
            {
                Nickname = userDTO.Nickname,
                Chats = userDTO.Chats,
                DateOfRegistration = DateTime.Now,
                Email = userDTO.Email,
                Password = userDTO.Password,
                MessageActivity = userDTO.MessageActivity,
                AverageMessageCountInDay = userDTO.AverageMessageCountInDay,
            };

            Database.Users.Create(user);
            await Database.Save();


            return new UserDTO
            {
                Id = user.Id,
                Chats = user.Chats,
                DateOfRegistration = user.DateOfRegistration,
                Email = user.Email,
                MessageActivity = user.MessageActivity,
                AverageMessageCountInDay = user.AverageMessageCountInDay,
                LastMessageTime = user.LastMessageTime,
                Nickname = user.Nickname
            };
        }

        public async Task AddUserToChatAsync(int userId, int chatId)
        {
            User user = await Database.Users.GetAsync(userId);
            Chat chat = await Database.Chats.GetAsync(chatId);

            if (chat != null | user != null)
            {
                chat.ChatUsers.Add(user);
                user.Chats.Add(chat);

                chat.ChatUsers.Add(user);
                user.Chats.Add(chat);
                await Database.Save();
            }
        }

        public async Task DelUserFromChatAsync(UserDTO userDTO, ChatDTO chatDTO)
        {
            User user = await Database.Users.GetAsync(userDTO.Id);
            Chat chat = await Database.Chats.GetAsync(chatDTO.Id);

            if (chat.ChatUsers.Find(c => c.Id == user.Id) != null)
            {
                chat.ChatUsers.Remove(user);

                if (chat.AdminId == user.Id & chat.ChatUsers.FirstOrDefault() != null)
                {
                    chat.AdminId = chat.ChatUsers.First().Id;
                }
                user.Chats.Remove(chat);
                await Database.Save();
            }
        }


        //TODO: MAPPER
        public async Task<Dictionary<UserDTO, int>> AllAnotherUsersAsync(int chatId)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            Dictionary<User, int> users = new();

            var chats = await Database.Chats.GetAllAsync();
            var chat = chats.First(c => c.Id == chatId);

            if (chat == null)
                return null;

            var allusers = await Database.Users.GetAllAsync();

            foreach (User user in allusers)
            {
                if (user.Chats.Find(c => c.Id == chat.Id) == null)
                    users.Add(user, chatId);
            }
            return mapper.Map<Dictionary<User, int>, Dictionary<UserDTO, int>>(users);
        }

        public async Task<List<ChatDTO>> GetAllUserChatsAsync(int userId)
        {
            List<ChatDTO> chatsDTO = new();
            var user = await Database.Users.GetAsync(userId);
            foreach (var chat in user.Chats)
            {
                Chat fullChat = await Database.Chats.GetAsync(chat.Id);

                if (fullChat == null)
                    return null;

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

        public async Task SaveMessageAsync(ChatMessageDTO messageDTO)
        {
            if (messageDTO != null)
            {
                ChatMessage message = new()
                {
                    Id = messageDTO.Id,
                    IsOld = messageDTO.isOld,
                    ChatId = messageDTO.ChatId,
                    Message = messageDTO.Message,
                    SenderId = messageDTO.SenderId,
                    Nickname = messageDTO.Nickname,
                    ChatName = messageDTO.ChatName,
                    TimeOfPosting = messageDTO.TimeOfPosting
                };
                Database.ChatMessages.Create(message);

                await UpdateUserLastMessageTimeAsync(messageDTO.SenderId, messageDTO.TimeOfPosting);
                await SaveMessageToQueueAsync(messageDTO);
                await Database.Save();
            }
        }

        private async Task UpdateUserLastMessageTimeAsync(int userId, DateTime timeOfPosting)
        {
            User user = Database.Users.Get(userId);
            user.LastMessageTime = timeOfPosting;
            user.MessageActivity++;
            //Database.Users.Update(user);
            await Database.Save();
        }

        public async Task SaveMessageToQueueAsync(ChatMessageDTO messageDTO)
        {
            if (messageDTO != null)
            {
                ChatMessageFIFO message = new()
                {
                    Id = messageDTO.Id,
                    ChatId = messageDTO.ChatId,
                    Message = messageDTO.Message,
                    Nickname = messageDTO.Nickname,
                    ChatName = messageDTO.ChatName,
                };
                Database.ChatMessagesFIFO.Create(message);
                await Database.Save();
            }
        }

        public async Task RemoveMessageAsync(int messageId)
        {
            if (messageId != 0)
            {
                Database.ChatMessages.Delete(messageId);
                await Database.Save();
            }
        }

        public async Task<ChatMessageFIFO> GetMessageFIFOAsync()
        {
            var messageFIFO = await Database.ChatMessagesFIFO.GetAllAsync();
            return messageFIFO.FirstOrDefault();
        }

        public async Task<List<ChatMessageFIFO>> GetAllMessagesFIFOAsync()
        {
            var allMesagesFIFO = await Database.ChatMessagesFIFO.GetAllAsync();
            return allMesagesFIFO.ToList();
        }

        public async Task RemoveMessageFIFOAsync()
        {
            var allMesFIFO = await Database.ChatMessagesFIFO.GetAllAsync();
            var firstMesFIFO = allMesFIFO.FirstOrDefault();

            if (firstMesFIFO != null)
                Database.ChatMessagesFIFO.Delete(firstMesFIFO.Id);
            await Database.Save();
        }

        public async Task RemoveMessageFIFOAsync(int messageId)
        {
            if (messageId != 0)
            {
                Database.ChatMessagesFIFO.Delete(messageId);
                await Database.Save();
            }
        }

        public async Task<ChatDTO> AddChatAsync(ChatDTO chatDTO)
        {
            if (chatDTO == null)
                return null;
            Chat chat = new() {
                AdminId = chatDTO.AdminId,
                ChatName = chatDTO.ChatName,
                ChatUsers = chatDTO.ChatUsers,
                ChatMessages = chatDTO.ChatMessages
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

        public async Task RemoveChatAsync(int chatId)
        {
            var chat = await Database.Chats.GetAsync(chatId);
            var users = await Database.Users.GetAllAsync();
            foreach (User user in users)
            {
                user.Chats.Remove(chat);
            }
            Database.Chats.Delete(chatId);
            await Database.Save();
        }

        public async Task<ChatMessageDTO> GetMessageAsync(int mesId)
        {
            var chatMessages = await Database.ChatMessages.GetAllAsync();
            var chatMessage = chatMessages.FirstOrDefault(x => x.Id == mesId);
            if (chatMessage == null)
                return null;
            return new()
            {
                Id = chatMessage.Id,
                isOld = chatMessage.IsOld,
                ChatId = chatMessage.ChatId,
                Message = chatMessage.Message,
                ChatName = chatMessage.ChatName,
                Nickname = chatMessage.Nickname,
                SenderId = chatMessage.SenderId,
                TimeOfPosting = chatMessage.TimeOfPosting
            };
        }

        private async Task<List<ChatMessage>> GetMessagesCheckOldAsync(int chatId)
        {
            var chatMessages = await Database.ChatMessages.GetAllAsync();
            foreach (ChatMessage chatMessage in chatMessages.Where(c => c.ChatId == chatId).ToList())
            {
                if (DateTime.Now >= chatMessage.TimeOfPosting.AddDays(+1))
                    chatMessage.IsOld = true;
                else
                    chatMessage.IsOld = false;
            }
            await Database.Save();

            return chatMessages
                .Where(c => c.ChatId == chatId)
                .ToList();
        }

        public async Task<List<ChatMessageDTO>> GetMessagesAsync(int chatId)
        {
            List<ChatMessage> chatMessages = await GetMessagesCheckOldAsync(chatId);
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
                    SenderId = chatMessage.SenderId,
                    Nickname = chatMessage.Nickname,
                    TimeOfPosting = chatMessage.TimeOfPosting
                });
            }

            return chatMessageDTOs;
        }

        public async Task<UserDTO> GetUserAsync(int userId)
        {
            User user = await Database.Users.GetAsync(userId);
            if (user == null)
                return null;
            return new UserDTO
            {
                Id = user.Id,
                Chats = user.Chats,
                Email = user.Email,
                Nickname = user.Nickname,
                Password = user.Password,
                LastMessageTime=user.LastMessageTime,
                MessageActivity = user.MessageActivity,
                AverageMessageCountInDay = user.AverageMessageCountInDay,
                DateOfRegistration = user.DateOfRegistration
            };
        }

        public async Task<ChatDTO> GetChatAsync(int chatId)
        {
            Chat chat = await Database.Chats.GetAsync(chatId);
            if (chat == null)
                return null;
            return new ChatDTO { AdminId = chat.AdminId, ChatName = chat.ChatName, Id = chat.Id };
        }





        public UserDTO GetUser(string email, string password)
        {
            var users = Database.Users.GetAll();
            User user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
                return null;
            else
                return new UserDTO
                {
                    Chats = user.Chats,
                    DateOfRegistration = user.DateOfRegistration,
                    Email = user.Email,
                    Id = user.Id,
                    MessageActivity = user.MessageActivity,
                    Nickname = user.Nickname,
                    AverageMessageCountInDay = user.AverageMessageCountInDay,
                    LastMessageTime= user.LastMessageTime
                };
        }

        public UserDTO GetUser(string nickname)
        {
            var users = Database.Users.GetAll();
            User user = users.FirstOrDefault(u => u.Nickname == nickname);

            if (user == null)
                return null;
            else
                return new UserDTO
                {
                    Chats = user.Chats,
                    DateOfRegistration = user.DateOfRegistration,
                    Email = user.Email,
                    Id = user.Id,
                    MessageActivity = user.MessageActivity,
                    Nickname = user.Nickname,
                    AverageMessageCountInDay=user.AverageMessageCountInDay,
                    LastMessageTime=user.LastMessageTime
                };
        }

        public UserDTO AddNewUser(UserDTO userDTO)
        {
            User user = new()
            {
                Nickname = userDTO.Nickname,
                Chats = userDTO.Chats,
                DateOfRegistration = DateTime.Now,
                Email = userDTO.Email,
                Password = userDTO.Password,
                MessageActivity = userDTO.MessageActivity,
                AverageMessageCountInDay= userDTO.AverageMessageCountInDay,
                LastMessageTime= userDTO.LastMessageTime
            };


            Database.Users.Create(user);
            Database.Save();


            return new UserDTO
            {
                Id = user.Id,
                Chats = user.Chats,
                DateOfRegistration = user.DateOfRegistration,
                Email = user.Email,
                MessageActivity = user.MessageActivity,
                Nickname = user.Nickname,
                AverageMessageCountInDay=user.AverageMessageCountInDay,
                LastMessageTime=user.LastMessageTime
            };
        }

        public void AddUserToChat(int userId, int chatId)
        {
            User user = Database.Users.Get(userId);
            Chat chat = Database.Chats.Get(chatId);

            if (chat != null | user != null)
            {
                chat.ChatUsers.Add(user);
                user.Chats.Add(chat);

                chat.ChatUsers.Add(user);
                user.Chats.Add(chat);
                Database.Save();
            }
        }

        public void DelUserFromChat(UserDTO userDTO, ChatDTO chatDTO)
        {
            User user = Database.Users.Get(userDTO.Id);
            Chat chat = Database.Chats.Get(chatDTO.Id);

            if (chat.ChatUsers.Find(c => c.Id == user.Id) != null)
            {
                chat.ChatUsers.Remove(user);

                if (chat.AdminId == user.Id & chat.ChatUsers.FirstOrDefault() != null)
                {
                    chat.AdminId = chat.ChatUsers.First().Id;
                }
                user.Chats.Remove(chat);
                Database.Save();
            }
        }

        public Dictionary<UserDTO, int> AllAnotherUsers(int chatId)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            Dictionary<User, int> users = new();

            var chats = Database.Chats.GetAll();
            var chat = chats.First(c => c.Id == chatId);

            if (chat == null)
                return null;

            var allusers = Database.Users.GetAll();

            foreach (User user in allusers)
            {
                if (user.Chats.Find(c => c.Id == chat.Id) == null)
                    users.Add(user, chatId);
            }
            return mapper.Map<Dictionary<User, int>, Dictionary<UserDTO, int>>(users);
        }

        public List<ChatDTO> GetAllUserChats(int userId)
        {
            List<ChatDTO> chatsDTO = new();
            var user = Database.Users.Get(userId);
            foreach (var chat in user.Chats)
            {
                Chat fullChat = Database.Chats.Get(chat.Id);

                if (fullChat == null)
                    return null;

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

        public void SaveMessage(ChatMessageDTO messageDTO)
        {
            if (messageDTO != null)
            {
                ChatMessage message = new()
                {
                    Id = messageDTO.Id,
                    IsOld = messageDTO.isOld,
                    ChatId = messageDTO.ChatId,
                    Message = messageDTO.Message,
                    SenderId = messageDTO.SenderId,
                    Nickname = messageDTO.Nickname,
                    ChatName = messageDTO.ChatName,
                    TimeOfPosting = messageDTO.TimeOfPosting
                };
                Database.ChatMessages.Create(message);
                UpdateUserLastMessageTime(messageDTO.SenderId, messageDTO.TimeOfPosting);
                SaveMessageToQueue(messageDTO);
                Database.Save();
            }
        }

        private void UpdateUserLastMessageTime(int userId, DateTime timeOfPosting)
        {
            Database.Users.Get(userId).LastMessageTime = timeOfPosting;
            Database.Save();
        }

        public void SaveMessageToQueue(ChatMessageDTO messageDTO)
        {
            if (messageDTO != null)
            {
                ChatMessageFIFO message = new()
                {
                    Id = messageDTO.Id,
                    ChatId = messageDTO.ChatId,
                    Message = messageDTO.Message,
                    Nickname = messageDTO.Nickname,
                    ChatName = messageDTO.ChatName,
                };
                Database.ChatMessagesFIFO.Create(message);
                Database.Save();
            }
        }

        public void RemoveMessage(int messageId)
        {
            if (messageId != 0)
            {
                Database.ChatMessages.Delete(messageId);
                Database.Save();
            }
        }

        public ChatMessageFIFO GetMessageFIFO()
        {
            var messageFIFO = Database.ChatMessagesFIFO.GetAll();
            return messageFIFO.FirstOrDefault();
        }

        public List<ChatMessageFIFO> GetAllMessagesFIFO()
        {
            var allMesagesFIFO = Database.ChatMessagesFIFO.GetAll();
            return allMesagesFIFO.ToList();
        }

        public void RemoveMessageFIFO()
        {
            var allMesFIFO = Database.ChatMessagesFIFO.GetAll();
            var firstMesFIFO = allMesFIFO.FirstOrDefault();

            if (firstMesFIFO != null)
                Database.ChatMessagesFIFO.Delete(firstMesFIFO.Id);
            Database.Save();
        }

        public void RemoveMessageFIFO(int messageId)
        {
            if (messageId != 0)
            {
                Database.ChatMessagesFIFO.Delete(messageId);
                Database.Save();
            }
        }

        public ChatDTO AddChat(ChatDTO chatDTO)
        {
            if (chatDTO == null)
                return null;
            Chat chat = new()
            {
                AdminId = chatDTO.AdminId,
                ChatName = chatDTO.ChatName,
                ChatUsers = chatDTO.ChatUsers,
                ChatMessages = chatDTO.ChatMessages
            };

            Database.Chats.Create(chat);
            Database.Save();

            return new ChatDTO
            {
                Id = chat.Id,
                AdminId = chat.AdminId,
                ChatName = chat.ChatName,
                ChatUsers = chat.ChatUsers,
                ChatMessages = chat.ChatMessages,
            };
        }

        public void RemoveChat(int chatId)
        {
            var chat = Database.Chats.Get(chatId);
            var users = Database.Users.GetAll();
            foreach (User user in users)
            {
                user.Chats.Remove(chat);
            }
            Database.Chats.Delete(chatId);
            Database.Save();
        }

        public ChatMessageDTO GetMessage(int mesId)
        {
            var chatMessages = Database.ChatMessages.GetAll();
            var chatMessage = chatMessages.FirstOrDefault(x => x.Id == mesId);
            if (chatMessage == null)
                return null;
            return new()
            {
                Id = chatMessage.Id,
                isOld = chatMessage.IsOld,
                ChatId = chatMessage.ChatId,
                Message = chatMessage.Message,
                ChatName = chatMessage.ChatName,
                SenderId = chatMessage.SenderId,
                Nickname = chatMessage.Nickname,
                TimeOfPosting = chatMessage.TimeOfPosting
            };
        }

        private List<ChatMessage> GetMessagesCheckOld(int chatId)
        {
            var chatMessages = Database.ChatMessages.GetAll();
            foreach (ChatMessage chatMessage in chatMessages.Where(c => c.ChatId == chatId).ToList())
            {
                if (DateTime.Now >= chatMessage.TimeOfPosting.AddDays(+1))
                    chatMessage.IsOld = true;
                else
                    chatMessage.IsOld = false;
            }
            Database.Save();

            return chatMessages
                .Where(c => c.ChatId == chatId)
                .ToList();
        }

        public List<ChatMessageDTO> GetMessages(int chatId)
        {
            List<ChatMessage> chatMessages = GetMessagesCheckOld(chatId);
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
            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                Password = user.Password,
                MessageActivity = user.MessageActivity,
                DateOfRegistration = user.DateOfRegistration,
                AverageMessageCountInDay=user.AverageMessageCountInDay,
                LastMessageTime=user.LastMessageTime,
                Chats=user.Chats
            };
        }

        public ChatDTO GetChat(int chatId)
        {
            Chat chat = Database.Chats.Get(chatId);
            if (chat == null)
                return null;
            return new ChatDTO { AdminId = chat.AdminId, ChatName = chat.ChatName, Id = chat.Id };
        }

        public void UpdateUser(UserDTO userDTO)
        {
            var user = Database.Users.Find(c => c.Id == userDTO.Id).First();
            user.Nickname = userDTO.Nickname;
            user.Chats = userDTO.Chats;
            user.DateOfRegistration = DateTime.Now;
            user.Email = userDTO.Email;
            user.Password = userDTO.Password;
            user.MessageActivity = userDTO.MessageActivity;
            user.AverageMessageCountInDay = userDTO.AverageMessageCountInDay;
            Database.Save();
        }
    }
}