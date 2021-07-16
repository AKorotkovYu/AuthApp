using System;
using OneChat.BLL.Interfaces;
using OneChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using OneChat.DAL.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;


namespace OneChat.BLL.Services
{
    public class Logic: ILogic
    {
        private readonly IStore store;
        
        public Logic(IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task SendAsync(ChatMessageDTO chatMessageDTO)
        {
            await store.SaveMessageAsync(chatMessageDTO);
            await store.SaveMessageAsync(chatMessageDTO);
            await store.SaveMessageAsync(chatMessageDTO);
            await store.SaveMessageAsync(chatMessageDTO);
        }

        public async Task DelMesAsync(int chatId, int mesId)
        {
            var message = await store.GetMessageAsync(mesId);
            if((message!=null)&(message.TimeOfPosting > DateTime.Now.AddDays(-1)))
                    await store.RemoveMessageAsync(mesId);
        }

        public async Task ExitAsync(int userId, int chatId)
        {
            var user = await store.GetUserAsync(userId);
            var chat = await store.GetChatAsync(chatId);
            if(user!=null & chat!=null)
                await store.DelUserFromChatAsync(user, chat);
        }

        public async Task CreateChatAsync(int id, ChatDTO chatDTO)
        {
            UserDTO userDTO = await store.GetUserAsync(id);

            if (userDTO != null)
            {
                ChatDTO newChat = new()
                {
                    ChatName = chatDTO.ChatName,
                    AdminId = userDTO.Id,
                    ChatMessages = new()
                };

                newChat = await store.AddChatAsync(newChat);

                newChat.ChatMessages.Add(new()
                {
                    ChatId = newChat.Id,
                    ChatName = newChat.ChatName,
                    Message = "чат создан",
                    Nickname = "system",
                    TimeOfPosting = System.DateTime.Now
                });
                await store.AddUserToChatAsync(id, newChat.Id);
            }
        }

        public void Send(ChatMessageDTO chatMessageDTO)
        {
             store.SaveMessage(chatMessageDTO);
        }

        public void DelMes(int chatId, int mesId)
        {
            var message = store.GetMessage(mesId);
            if ((message != null) & (message.TimeOfPosting > DateTime.Now.AddDays(-1)))
                 store.RemoveMessage(mesId);
        }

        public void Exit(int userId, int chatId)
        {
            var user = store.GetUser(userId);
            var chat = store.GetChat(chatId);
            if (user != null & chat != null)
                store.DelUserFromChat(user, chat);
        }

        public void CreateChat(int id, ChatDTO chatDTO)
        {
            UserDTO userDTO = store.GetUser(id);

            if (userDTO != null)
            {
                ChatDTO newChat = new()
                {
                    ChatName = chatDTO.ChatName,
                    AdminId = userDTO.Id,
                    ChatMessages = new()
                };

                newChat = store.AddChat(newChat);

                newChat.ChatMessages.Add(new()
                {
                    ChatId = newChat.Id,
                    ChatName = newChat.ChatName,
                    Message = "чат создан",
                    Nickname = "system",
                    TimeOfPosting = System.DateTime.Now
                });
                store.AddUserToChat(id, newChat.Id);
            }
        }

        public void UpdateUser(UserDTO user)
        {
            store.UpdateUser(user);
        }
    }
}
