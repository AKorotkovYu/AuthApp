using OneChat.BLL.Interfaces;
using System.Threading.Tasks;


namespace OneChat.BLL.BusinessModel
{
    public class DownloadBot : IBot
    {
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
