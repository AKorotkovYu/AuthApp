using System;
using System.Collections.Generic;
using System.Linq;
using OneChat.BLL.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using OneChat.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace OneChat.BLL.BusinessModel
{
    public class JokeBot : IBot
    {
        private readonly List<string> phrases = new();
        private readonly List<string> jokes = new();
        private readonly Random randomizer = new();

        private readonly IServiceProvider serviceProvider;

        public JokeBot(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;


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



        public async Task CheckMessages(ChatMessageFIFO chatMessage)
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
