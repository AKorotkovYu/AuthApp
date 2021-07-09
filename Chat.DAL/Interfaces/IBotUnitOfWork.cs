using System;
using System.Threading.Tasks;
using OneChat.DAL.Entities;

namespace OneChat.DAL.Interfaces
{
    public interface IBotUnitOfWork : IDisposable
    {
        IRepository<ChatMessage> ChatMessages { get; }
        Task Save();
    }
}
