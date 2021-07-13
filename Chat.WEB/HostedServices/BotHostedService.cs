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

namespace OneChat.WEB.HostedServices
{
    public class BotHostedService: BackgroundService
    {

        private readonly IStore store;
        private readonly List<IBot> bots;
        private readonly List<Task> tasks;
        private IConfiguration AppConfiguration { get; set; }

        public BotHostedService(IStore store, IConfiguration configuration, IConfiguration conf)
        {
            tasks = new List<Task>();
            AppConfiguration = conf;


            this.store = store ?? throw new ArgumentNullException(nameof(store));
            MyServiceCollection sc = new(configuration);
            bots = sc.AddConfig().ToList();
        }



        protected override Task ExecuteAsync(CancellationToken token)
        {
            return Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await CheckFIFOAsync();
                    await Task.Delay(1000, token);
                }

            }, token);
        }


        /// <summary>
        /// Распределение многопоточной работы над сообщением в FIFO
        /// </summary>
        /// <returns></returns>
        private async Task CheckFIFOAsync()
        {

            var MaxThreads = Int32.Parse(AppConfiguration["BotsSettings:WorkerThread"]);

            List<ChatMessageFIFO> listFIFO = store.GetAllMessagesFIFO();

            foreach (var chatMessageFIFO in listFIFO)
            {
                foreach (var bot in bots)
                {
                    if (tasks.Count < MaxThreads)
                    {
                        tasks.Add(bot.CheckMessages(chatMessageFIFO));
                    }
                    else
                    {
                        Task.WaitAny(tasks.ToArray());
                        tasks.Remove(tasks.First(c => c.IsCompleted == true));
                        tasks.Add(bot.CheckMessages(chatMessageFIFO));
                    }
                }
            }

            //Дожидаемся всех чтобы перейти к следующему сообщению
            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            foreach (var chatMessageFIFO in listFIFO)
                await store.RemoveMessageFIFO();
        }
    }
}
