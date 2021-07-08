using System;
using OneChat.BLL.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using OneChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace OneChat.BLL.BusinessModel
{
   internal class TimeBot : IBot
    {
        public string Name => "Timer";

        IServiceScopeFactory serviceScopeFactory;

        public TimeBot(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task CheckMessage(ChatMessageDTO chatMessageDTO)
        {
            var tasks = new List<Task>
            {
                await this.ExecuteAsync(chatMessageDTO.Message).ContinueWith(async (task) =>
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IStore>();
                    if (!string.IsNullOrEmpty(task.Result))
                        await repository.SaveMessage(new()
                        {
                            ChatId = chatMessageDTO.ChatId,
                            ChatName = chatMessageDTO.ChatName,
                            Message = task.Result,
                            Nickname = this.Name,
                            TimeOfPosting = System.DateTime.Now,
                        });
                })
            };
        }

        public async Task<string> ExecuteAsync(string message)
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
