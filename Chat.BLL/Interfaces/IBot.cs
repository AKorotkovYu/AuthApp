using System.Threading.Tasks;
using OneChat.DAL.Entities;

namespace OneChat.BLL.Interfaces
{
    public interface IBot
    {
        public string Name { get; }
        public Task<string> ExecuteAsync(string message);
        public Task CheckMessages(ChatMessageFIFO chatMessageFIFO);
    }
}
