using System.Threading.Tasks;
using OneChat.DAL.Entities;

namespace OneChat.BLL.Interfaces
{
    public interface IBot
    {
        public abstract string Name { get; }
        public abstract Task<string> ExecuteAsync(string message);
        public abstract Task CheckMessages(ChatMessageFIFO chatMessageFIFO);
    }
}
