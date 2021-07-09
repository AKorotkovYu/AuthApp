using System.Collections.Generic;
using System.Threading.Tasks;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;

namespace OneChat.BLL.Interfaces
{
    public interface IBotStore
    {
        public Task SaveMessage(ChatMessageDTO messageDTO);
    }
}
