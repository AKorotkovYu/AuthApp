using System;
using OneChat.BLL.Interfaces;
using System.Threading.Tasks;

namespace OneChat.BLL.BusinessModel
{
    public class TimeBot : IBot
    {
        public string Name => "Timer";

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
