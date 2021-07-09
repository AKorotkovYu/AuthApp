using OneChat.DAL.Interfaces;
using OneChat.BLL.Interfaces;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;
using System.Threading.Tasks;

namespace OneChat.BLL.Services
{
    public class BotStore: IBotStore
    {
        IBotUnitOfWork Database { get; set; }

        public BotStore(IBotUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task SaveMessage(ChatMessageDTO messageDTO)
        {
            ChatMessage message = new()
            {
                Id = messageDTO.Id,
                IsOld = messageDTO.isOld,
                ChatId = messageDTO.ChatId,
                Message = messageDTO.Message,
                Nickname = messageDTO.Nickname,
                ChatName = messageDTO.ChatName,
                TimeOfPosting = messageDTO.TimeOfPosting
            };
            Database.ChatMessages.Create(message);
            await Database.Save();
        }
    }


    
}
