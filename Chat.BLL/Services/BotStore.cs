using OneChat.DAL.Interfaces;
using OneChat.BLL.Interfaces;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using AutoMapper;

namespace OneChat.BLL.Services
{
    public class BotStore: IBotStore
    {
        IBotUnitOfWork Database { get; set; }
        IMapper mapper { get; set; }


        public BotStore(IBotUnitOfWork uow, IMapper meapper)
        {
            Database = uow;
            this.mapper = mapper;
        }

        public async Task SaveMessage(ChatMessageDTO messageDTO)
        {
            ChatMessage message = mapper.Map<ChatMessage>(messageDTO);
            Database.ChatMessages.Create(message);
            await Database.Save();
        }
    }


    
}
