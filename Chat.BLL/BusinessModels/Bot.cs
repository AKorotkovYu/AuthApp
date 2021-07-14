using System.Threading.Tasks;
using OneChat.BLL.Interfaces;
using OneChat.DAL.Entities;

namespace OneChat.BLL.BusinessModel
{
    public abstract class Bot : IBot
    {
        public abstract string Name { get; }
        public abstract Task<string> ExecuteAsync(string message);
        public abstract Task<ChatMessageFIFO> CheckMessages(ChatMessageFIFO chatMessage);
    }
}
