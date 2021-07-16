using System.Threading.Tasks;
using OneChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace OneChat.BLL.Interfaces
{
    public interface ILogic
    {
        public Task SendAsync(ChatMessageDTO model);
        public Task DelMesAsync(int chatId, int mesId);
        public Task ExitAsync(int userId, int chatId);
        public Task CreateChatAsync(int id, ChatDTO chatModel);

        public void Send(ChatMessageDTO model);
        public void DelMes(int chatId, int mesId);
        public void Exit(int userId, int chatId);
        public void CreateChat(int id, ChatDTO chatModel);

        public void UpdateUser(UserDTO user);
    }
}
