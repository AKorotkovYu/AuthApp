using System.Collections.Generic;
using System.Threading.Tasks;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;

namespace OneChat.BLL.Interfaces
{
    public interface IStore
    {

        public Task<UserDTO> AddNewUser(UserDTO userDTO);
        public Task AddUserToChat(int userId, int chatId);
        public Task DelUserFromChat(int userId, int chatId);
        public UserDTO GetUser(string Email, string Password);
        public UserDTO GetUser(string nickname);

        public UserDTO GetUser(int userId);
        public ChatDTO GetChat(int chatId);

        public Dictionary<UserDTO, int> AllAnotherUsers(int chatId);
        public List<ChatDTO> GetAllUserChats(int userId);
        
        public ChatMessageDTO GetMessage(int mesId);
        public Task SaveMessage(ChatMessageDTO messageDTO);

        public Task<List<ChatMessageDTO>> GetMessages(int chatId);

        public Task<ChatDTO> AddChat(ChatDTO chat);
        public Task RemoveChat(int chatId);
        public Task RemoveMessage(int messageId);
        public Task RemoveMessageFIFO();
        public ChatMessageFIFO GetMessageFIFO();
        public List<ChatMessageFIFO> GetAllMessagesFIFO();
        public Task RemoveMessageFIFO(int messageId);
    }
}
