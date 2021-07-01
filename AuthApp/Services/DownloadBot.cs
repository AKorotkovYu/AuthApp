using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Services
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
