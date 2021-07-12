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
        private readonly List<IBot> bots;
        private readonly List<Task> tasks;
        

        public Logic(IStore store, IConfiguration configuration)
        {
            tasks = new List<Task>();
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            MyServiceCollection sc = new(configuration);
            bots = sc.AddConfig().ToList();
        }



        public async Task Send(ChatMessageDTO chatMessageDTO)
        {
            await store.SaveMessage(chatMessageDTO);
        }



        public async Task CheckFIFOAsync()
        {
            ChatMessageFIFO chatMessageFIFO = store.GetMessageFIFO();
            if(chatMessageFIFO!=null)
                
            do
            {    
                foreach (IBot bot in bots)
                {
                    tasks.Add(bot.CheckMessages(chatMessageFIFO));
                }

                Task.WaitAll(tasks.ToArray());
                tasks.Clear();

                await store.RemoveMessageFIFO();
                chatMessageFIFO = store.GetMessageFIFO();
            } 
            while (chatMessageFIFO != null);
        }



        public async Task DelMes(int chatId, int mesId)
        {
            if (store.GetMessage(mesId).TimeOfPosting > DateTime.Now.AddDays(-1))
                await store.RemoveMessage(mesId);
        }



        public async Task Exit(int userId, int chatId)
        {
            await store.DelUserFromChat(userId, chatId);
        }



        public async Task CreateChat(int id, ChatDTO chatDTO)
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
}
