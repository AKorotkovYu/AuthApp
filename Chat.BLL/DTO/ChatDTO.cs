using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneChat.DAL.Entities;

namespace OneChat.BLL.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public string ChatName { get; set; }
        public int AdminId { get; set; }
        public List<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public List<User> ChatUsers { get; set; } = new List<User>();
    }
}
