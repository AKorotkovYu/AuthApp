using System;
using OneChat.BLL.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using OneChat.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace OneChat.BLL.BusinessModel
{
   public class TimeBot : Bot
    {
        
        private readonly IServiceProvider serviceProvider;

        public TimeBot(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override async Task<ChatMessageFIFO> CheckMessages(ChatMessageFIFO chatMessage)
        {
            await this.ExecuteAsync(chatMessage.Message).ContinueWith(async (task) =>
            {
                var repository = serviceProvider.GetRequiredService<IBotStore>();
                if (!string.IsNullOrEmpty(task.Result))
                {
                    await repository.SaveMessage(new()
                    {
                        ChatId = chatMessage.ChatId,
                        ChatName = chatMessage.ChatName,
                        Message = task.Result,
                        Nickname = $"{this.Name} {task.Id}",
                        TimeOfPosting = System.DateTime.Now,
                    });
                }
            });
            return chatMessage;
        }

        public override string Name => "Timer";

        public override async Task<string> ExecuteAsync(string message)
        {
            return await Task.Run(() => Execute(message));
        }



        public string Execute(string message)
        {
            if (message != null)
            {
                if (message == @"/current")
                {
                    string answer = DateTime.Now.ToString();
                    return answer;
                }
                var splittedMessage = message.Split(" ");
                if (splittedMessage.Length >= 2)
                {
                    if (splittedMessage[0] == "через")
                    {
                        if (Int32.TryParse(splittedMessage[1], out int min))
                            return $"Через {min} минут будет {DateTime.Now.AddMinutes(min)}";
                    }
                }
            }
            return null;
        }

    }
}
