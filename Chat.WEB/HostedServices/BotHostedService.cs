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
        private int tasksCount = 0;
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
            ChatMessageFIFO chatMessageFIFO = store.GetMessageFIFO();
            
            while(chatMessageFIFO!=null)
            {
                foreach(var bot in bots)
                {
                    if (tasksCount < MaxThreads)
                    {             
                        tasks.Add(bot.CheckMessages(chatMessageFIFO));
                        tasksCount++;
                    }

                    if(tasksCount == MaxThreads)
                    {
                        Task.WaitAny(tasks.ToArray());
                        tasks.Remove(tasks.First(c => c.IsCompleted == true));
                        tasks.Add(bot.CheckMessages(chatMessageFIFO));
                    }
                }


                //Дожидаемся всех чтобы перейти к следующему сообщению
                Task.WaitAll(tasks.ToArray());
                tasksCount = 0;
                tasks.Clear();

                await store.RemoveMessageFIFO();
                chatMessageFIFO = store.GetMessageFIFO();
            }
        }
    }
}
