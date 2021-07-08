using System;
using OneChat.BLL.Interfaces;
using OneChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OneChat.BLL.Services
{
    public class Logic: ILogic
    {
        private readonly IStore store;

        public delegate Task MethodContainer(ChatMessageDTO chatMessageDTO);
        public event MethodContainer OnSend;


        public Logic(IServiceProvider serviceProvider, IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
                  
            foreach (IBot bot in serviceProvider.GetServices<IBot>())
                this.OnSend += bot.CheckMessage;
        }


        public async Task Send(ChatMessageDTO chatMessageDTO)
        {
            await store.SaveMessage(chatMessageDTO);
            await this.OnSend(chatMessageDTO);
        }

        public async Task DelMes(int chatId, int mesId)
        {
            if(store.GetMessage(mesId).TimeOfPosting>DateTime.Now.AddDays(-1))
                await store.RemoveMessage(mesId);
        }

        public async Task Exit(int userId, int chatId)
        {
            await store.DelUserFromChat(userId, chatId);
        }

        public async Task CreateChat(int id, ChatDTO chatDTO)
        {
            UserDTO userDTO = store.GetUser(id);

            ChatDTO newChat = new()
            {
                ChatName = chatDTO.ChatName,
                AdminId = userDTO.Id,
                ChatMessages = new()
            };

            newChat = await store.AddChat(newChat);

            newChat.ChatMessages.Add(new()
            {
                ChatId = newChat.Id,
                ChatName = newChat.ChatName,
                Message = "чат создан",
                Nickname = "system",
                TimeOfPosting = System.DateTime.Now
            });
            await store.AddUserToChat(id, newChat.Id);
        }
    }
}
