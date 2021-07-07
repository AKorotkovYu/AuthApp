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
        private readonly IServiceProvider serv;
        private readonly IStore store;

        public Logic(IServiceProvider serv, IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.serv = serv ?? throw new ArgumentNullException(nameof(serv));
        }

        public async Task Send(ChatMessageDTO chatMessageDTO, IServiceScopeFactory serviceScopeFactory)
        {
            var bots = serv.GetServices<IBot>();
            var tasks = new List<Task>();

            await store.SaveMessage(chatMessageDTO);
            foreach (var bot in bots)
            {
                tasks.Add(bot.ExecuteAsync(chatMessageDTO.Message).ContinueWith(async (task) =>
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IStore>();
                    if (!string.IsNullOrEmpty(task.Result))
                        await repository.SaveMessage(new()
                        {
                            ChatId = chatMessageDTO.ChatId,
                            ChatName = chatMessageDTO.ChatName,
                            Message = task.Result,
                            Nickname = bot.Name,
                            TimeOfPosting = System.DateTime.Now,
                        });
                }));
                Task.WaitAny(tasks.ToArray());
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
            await store.AddUserToChat(id, newChat.Id);
        }
    }
}
