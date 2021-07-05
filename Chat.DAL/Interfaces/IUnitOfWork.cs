using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneChat.DAL.Entities;

namespace OneChat.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Chat> Chats { get; }
        IRepository<ChatMessage> ChatMessages { get; }
        IRepository<User> Users { get; }
        void Save();
    }
}
