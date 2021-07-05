using OneChat.BLL.Interfaces;

namespace OneChat.BLL.BusinessModel
{
    public class DownloadBot: IBot
    {
        public string Name => "Downloader";

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
