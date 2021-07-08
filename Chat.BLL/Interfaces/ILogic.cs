using System.Threading.Tasks;
using OneChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace OneChat.BLL.Interfaces
{
    public interface ILogic
    {
        public Task Send(ChatMessageDTO model);
        public Task DelMes(int chatId, int mesId);
        public Task Exit(int userId, int chatId);
        public Task CreateChat(int id, ChatDTO chatModel);
    }
}
