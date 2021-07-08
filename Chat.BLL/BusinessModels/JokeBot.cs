using System;
using System.Collections.Generic;
using System.Linq;
using OneChat.BLL.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using OneChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace OneChat.BLL.BusinessModel
{
    internal class JokeBot : IBot
    {
        private readonly List<string> phrases = new();
        private readonly List<string> jokes = new();
        private readonly Random randomizer = new();
        List<Task> tasks;

        IServiceScopeFactory serviceScopeFactory;

        public JokeBot(IServiceScopeFactory serviceScopeFactory)
        {
            tasks = new List<Task>();
            this.serviceScopeFactory = serviceScopeFactory;
            phrases.Add("скучно");
            phrases.Add("грустно");

            jokes.Add("Шутка 1");
            jokes.Add("Шутка 2");
            jokes.Add("Шутка 3");
            jokes.Add("Шутка 4");
            jokes.Add("Шутка 5");
            jokes.Add("Шутка 6");
            jokes.Add("Шутка 7");
            jokes.Add("Шутка 8");
            jokes.Add("Шутка 9");
        }

        public async Task CheckMessage(ChatMessageDTO chatMessageDTO)
        {
            tasks.Add(
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
                }));
        }




        public string Name => "Joker";

        public Task<string> ExecuteAsync(string message) => Task.Run(() => Execute(message));

        public string Execute(string message)
        {
            Thread.Sleep(15000);
            if (message != null)
            {
                var splittedMessage = message.Split();
                foreach (var spltMsg in splittedMessage)
                    foreach (var phrase in phrases)
                        if (spltMsg == phrase)
                        {
                            string answer = TakeRandAnswer(jokes);
                            return answer;
                        }
            }
            return null;
        }

        string TakeRandAnswer(List<string> lAnswer)
        {

            if (lAnswer.Count == 0)
                return "ERROR";
            int r = randomizer.Next(0, lAnswer.Count);
            var an1 = lAnswer.Skip(r).Take(1).ToArray();

            return an1.FirstOrDefault();
        }
    }
}
