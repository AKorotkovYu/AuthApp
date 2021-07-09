using OneChat.BLL.Interfaces;
using System.Threading.Tasks;
using OneChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;


namespace OneChat.BLL.BusinessModel
{
    public class DownloadBot : IBot
    {

        IServiceScopeFactory serviceScopeFactory;

        public DownloadBot(IServiceScopeFactory serviceScopeFactory)
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
                    var repository = scope.ServiceProvider.GetRequiredService<IBotStore>();
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
