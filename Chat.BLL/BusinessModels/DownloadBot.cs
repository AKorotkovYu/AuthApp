using System;
using System.Collections.Generic;
using OneChat.BLL.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneChat.DAL.Entities;

namespace OneChat.BLL.BusinessModel
{
    public class DownloadBot : IBot
    {
        private readonly IServiceProvider serviceProvider;



        public DownloadBot(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }



        public async Task CheckMessages(ChatMessageFIFO chatMessage)
        {
            await this.ExecuteAsync(chatMessage.Message).ContinueWith(async (task) =>
            {
                var repository = serviceProvider.GetRequiredService<IBotStore>();
                if (!string.IsNullOrEmpty(task.Result))
                    await repository.SaveMessage(new()
                    {
                        ChatId = chatMessage.ChatId,
                        ChatName = chatMessage.ChatName,
                        Message = task.Result,
                        Nickname = this.Name,
                        TimeOfPosting = System.DateTime.Now,
                    });
            });
        }

        public string Name => "Downloader";



        public async Task<string> ExecuteAsync(string message)
        {
            return await Task.Run(() => Execute(message));
        }



        public string Execute(string message)
        {
            if (message == @"/download")
            {
                /*логика*/
                string answer = "какой-то текст";

                return answer;
            }
            return null;
        }
    }
}
