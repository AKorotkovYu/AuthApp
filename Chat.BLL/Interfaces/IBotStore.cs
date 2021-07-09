using System.Collections.Generic;
using System.Threading.Tasks;
using OneChat.BLL.DTO;

namespace OneChat.BLL.Interfaces
{
    public interface IBotStore
    {
        public Task SaveMessage(ChatMessageDTO messageDTO);
    }
}
