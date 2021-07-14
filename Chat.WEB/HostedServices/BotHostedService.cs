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
using Microsoft.Extensions.Options;

namespace OneChat.WEB.HostedServices
{
    public class BotHostedService: BackgroundService
    {

        private readonly IStore store;
        private readonly List<IBot> bots;
        private readonly List<Task<ChatMessageFIFO>> tasks;
        private IConfiguration AppConfiguration { get; set; }


        static Mutex mutexObj = new Mutex();



        public BotHostedService(IStore store, IConfiguration configuration)
        {
            tasks = new List<Task<ChatMessageFIFO>>();

            this.store = store ?? throw new ArgumentNullException(nameof(store));

            //скидываем ботов
            MyServiceCollection sc = new(configuration);
            bots = sc.AddConfig().ToList();

            AppConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }



        protected override Task ExecuteAsync(CancellationToken token)
        {
            return Task.Run(async () => { await DistributeFIFOAsync(token); }, token);
        }



        private async Task DistributeFIFOAsync(CancellationToken token)
        {


            var allMessages = store.GetAllMessagesFIFO();
            var MaxThreads = Int32.Parse(AppConfiguration["BotsSettings:WorkerThread"]);
            

            //циклим токеном
            while (!token.IsCancellationRequested)
            {
                if (allMessages.Count != 0)
                {
                    //скидываем всем ботам все сообщения
                    foreach (var message in allMessages)
                    {
                        foreach (var bot in bots)
                        {
                            //если есть 4 потока, занятых прямо сейчас, то ждём хоть один
                            while (tasks.Where(c => c.IsCompleted == false).Count() >= MaxThreads)
                            {
                                Task.WaitAny(tasks.ToArray(), token);

                                //добавляем сообщению в флаг инфу о том, что оно обработано
                                allMessages.Find(c => c.Id == tasks.Find(c => c.IsCompleted == true).Result.Id).InProcess++;
                                tasks.Remove(tasks.Find(c => c.IsCompleted == true));
                            }
                            tasks.Add(bot.CheckMessages(message));
                        }
                    }



                    //заканчиваем обработку этого пака сообщений(последние четыре таски этого пака сообщений. медленные боты или просто последнее)
                    while (tasks.Where(c => c.IsCompleted == false).Any())
                    {
                        Task.WaitAny(tasks.ToArray(), token);
                        allMessages.Find(c => c.Id == tasks.Find(c => c.IsCompleted == true).Result.Id).InProcess++;
                        tasks.Remove(tasks.Find(c => c.IsCompleted == true));
                    }



                    //Просматриваем на прохождение обработки всех ботов для удаления
                    foreach (var message in allMessages)
                        if (message.InProcess >= bots.Count)
                        {
                            await store.RemoveMessageFIFO(message.Id);
                        }
                }

                //циклим
                allMessages = store.GetAllMessagesFIFO();
                await Task.Delay(25000, token);
            }
        }
    }
}
