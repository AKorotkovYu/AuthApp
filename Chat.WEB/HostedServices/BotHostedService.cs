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
    public class BotHostedService : BackgroundService
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
            return Task.Run(async () => {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(2500);
                    await DistributeFIFOAsync1(token);
                }
            }, token);
        }



        private async Task DistributeFIFOAsync1(CancellationToken token)
        {
            var allMessagesInBase = await store.GetAllMessagesFIFOAsync();
            var MaxThreads = Int32.Parse(AppConfiguration["BotsSettings:WorkerThread"]);


            //циклим токеном
            while (!token.IsCancellationRequested)
            {
                if (allMessagesInBase.Count != 0)
                {
                    //тк важно чтобы он не прерывался
                    //скидываем всем ботам все сообщения

                    //чтобы мочь добавлять и изменять массив над которым работаем. Иначе выпадем в foreach
                    List<ChatMessageFIFO> allMessagesBuffer = new();
                    
                    foreach(var message in allMessagesInBase)
                        allMessagesBuffer.Add(message);



                    foreach (var message in allMessagesBuffer)
                    {
                       
                        //если мы их ещё не кидали на обработку в прошлом подходе. обычно это поледнее сообщение
                        if(message.InProcess==0)
                            foreach (var bot in bots)
                            {
                                //если есть 4 потока, занятых прямо сейчас, то ждём хоть один
                                while (tasks.Where(c => c.IsCompleted == false).Count() >= MaxThreads)
                                {
                                    Task.WaitAny(tasks.ToArray(), token);

                                    // проверяем и удаляем отработанные сообщения
                                    // каждый раз как добавляем таску
                                    foreach (var oneMessage in allMessagesBuffer)
                                    {
                                        if (oneMessage.InProcess >= bots.Count)
                                        {
                                            await store.RemoveMessageFIFOAsync(oneMessage.Id);
                                            allMessagesInBase.Remove(oneMessage);
                                        }
                                    }



                                    //перекидываем из буфера отработанное в данной законченной таске сообщение
                                    var processedMessage=allMessagesBuffer?.Find(c => c.Id == tasks.Find(c => c.IsCompleted == true).Result.Id);
                                    if (processedMessage != null)
                                    {
                                        var processedMessageInBase = allMessagesInBase?.Find(c => c.Id == processedMessage.Id);
                                        if (processedMessageInBase != null)
                                            processedMessageInBase.InProcess++;
                                    }
                                    tasks.Remove(tasks.Find(c => c.IsCompleted == true));
                                }

                                //кидаем в обработку
                                tasks.Add(bot.CheckMessages(message));     
                            }
                    }
                }
                allMessagesInBase = await store.GetAllMessagesFIFOAsync();
            }
        }
    }
}
