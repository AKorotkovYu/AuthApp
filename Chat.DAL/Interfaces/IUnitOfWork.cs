using System;
using System.Threading.Tasks;
using OneChat.DAL.Entities;

namespace OneChat.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Chat> Chats { get; }
        IRepository<ChatMessage> ChatMessages { get; }
        IRepository<User> Users { get; }
        IRepository<ChatMessageFIFO> ChatMessagesFIFO { get; }
        Task Save();
    }
}
