using AutoMapper;
using OneChat.BLL.DTO;
using OneChat.DAL.Entities;

namespace OneChat.WEB
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<ChatMessageDTO, ChatMessage>();
            CreateMap<ChatMessage, ChatMessageDTO>();
            CreateMap<ChatMessageFIFO, ChatMessageDTO>();
            CreateMap<ChatDTO, Chat>();
            CreateMap<Chat, ChatDTO>();
        }
    }
}
