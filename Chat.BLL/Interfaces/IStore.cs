using System.Collections.Generic;
using System.Threading.Tasks;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;

namespace OneChat.BLL.Interfaces
{
    public interface IStore
    {

        public Task<UserDTO> AddNewUserAsync(UserDTO userDTO);
        public Task AddUserToChatAsync(int userId, int chatId);
        public Task DelUserFromChatAsync(UserDTO userId, ChatDTO chatId);
        public Task<UserDTO> GetUserAsync(string Email, string Password);
        public Task<UserDTO> GetUserAsync(string nickname);
        public Task<UserDTO> GetUserAsync(int userId);
        public Task<ChatDTO> GetChatAsync(int chatId);
        public Task<Dictionary<UserDTO, int>> AllAnotherUsersAsync(int chatId);
        public Task<List<ChatDTO>> GetAllUserChatsAsync(int userId);
        public Task<ChatMessageDTO> GetMessageAsync(int mesId);
        public Task SaveMessageAsync(ChatMessageDTO messageDTO);
        public Task<List<ChatMessageDTO>> GetMessagesAsync(int chatId);
        public Task<ChatDTO> AddChatAsync(ChatDTO chat);
        public Task RemoveChatAsync(int chatId);
        public Task RemoveMessageAsync(int messageId);
        public Task RemoveMessageFIFOAsync();
        public Task RemoveMessageFIFOAsync(int id);
        public Task<ChatMessageFIFO> GetMessageFIFOAsync();
        public Task<List<ChatMessageFIFO>> GetAllMessagesFIFOAsync();




        public UserDTO AddNewUser(UserDTO userDTO);
        public void AddUserToChat(int userId, int chatId);
        public void DelUserFromChat(UserDTO userId, ChatDTO chatId);
        public UserDTO GetUser(string Email, string Password);
        public UserDTO GetUser(string nickname);
        public UserDTO GetUser(int userId);
        public ChatDTO GetChat(int chatId);
        public Dictionary<UserDTO, int> AllAnotherUsers(int chatId);
        public List<ChatDTO> GetAllUserChats(int userId);
        public ChatMessageDTO GetMessage(int mesId);
        public void SaveMessage(ChatMessageDTO messageDTO);
        public List<ChatMessageDTO> GetMessages(int chatId);
        public ChatDTO AddChat(ChatDTO chat);
        public void RemoveChat(int chatId);
        public void RemoveMessage(int messageId);
        public void RemoveMessageFIFO();
        public void RemoveMessageFIFO(int id);
        public ChatMessageFIFO GetMessageFIFO();
        public List<ChatMessageFIFO> GetAllMessagesFIFO();

        public void UpdateUser(UserDTO userDTO);
    }
}
