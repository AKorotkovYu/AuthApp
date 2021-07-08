using System.Threading.Tasks;
using OneChat.BLL.DTO;

namespace OneChat.BLL.Interfaces
{
    internal interface IBot
    {
        public abstract string Name { get; }
        public abstract Task<string> ExecuteAsync(string message);
        public abstract Task CheckMessage(ChatMessageDTO chatMessageDTO);
    }
}
