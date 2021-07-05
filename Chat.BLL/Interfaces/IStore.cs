using System.Collections.Generic;
using System.Threading.Tasks;
using OneChat.DAL.EF;
using OneChat.BLL.DTO;

namespace OneChat.BLL.Interfaces
{
    public interface IStore
    {

        public UserDTO AddNewUser(UserDTO userDTO);
        public void AddUserToChat(int userId, int chatId);
        public void DelUserFromChat(int userId, int chatId);
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

        public void SaveChanges();
    }
}
