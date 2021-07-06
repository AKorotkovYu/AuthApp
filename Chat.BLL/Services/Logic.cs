using System;
using OneChat.BLL.Interfaces;
using OneChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace OneChat.BLL.Services
{
    public class Logic: ILogic
    {
        private readonly IServiceProvider serv;
        private readonly IStore store;

        public Logic(IServiceProvider serv, IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.serv = serv ?? throw new ArgumentNullException(nameof(serv));
        }

        public async Task Send(ChatMessageDTO chatMessageDTO)
        {
            await store.SaveMessage(chatMessageDTO);
            await Task.Run(() => botAnswer(chatMessageDTO));
        }

        public void botAnswer(ChatMessageDTO chatMessageDTO)
        {
            var bots = serv.GetServices<IBot>();
            foreach (var bot in bots)
            {
                string answer = bot.Execute(chatMessageDTO.Message);
                if (answer != null)
                {
                    store.SaveMessage(new()
                    {
                        ChatId = chatMessageDTO.ChatId,
                        ChatName = chatMessageDTO.ChatName,
                        Message = answer,
                        Nickname = bot.Name,
                        TimeOfPosting = System.DateTime.Now
                    });
                }
            }
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
            store.AddUserToChat(id, newChat.Id);
        }
    }
}
