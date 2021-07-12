using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using OneChat.BLL.Interfaces;
using OneChat.DAL.Entities;
using Microsoft.Extensions.Configuration;
using System.Linq;
using OneChat.BLL.Services;
using System.Collections.Generic;

namespace OneChat.WEB
{
    public class BotHostedService: BackgroundService
    {

        private readonly IStore store;
        private readonly List<IBot> bots;
        private readonly List<Task> tasks;


        public BotHostedService(IStore store, IConfiguration configuration)
        {
            tasks = new List<Task>();
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            MyServiceCollection sc = new(configuration);
            bots = sc.AddConfig().ToList();
        }

        protected async override Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {    
                await CheckFIFOAsync();
                await Task.Delay(1000, token);
            }
        }

        public async Task CheckFIFOAsync()
        {
            
            ChatMessageFIFO chatMessageFIFO = store.GetMessageFIFO();
            if (chatMessageFIFO != null)

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
    }
}
