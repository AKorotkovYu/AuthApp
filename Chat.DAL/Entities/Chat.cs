using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneChat.DAL.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public string ChatName { get; set; }
        public List<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public List<User> ChatUsers { get; set; } = new List<User>();
        public int AdminId { get; set; }
    }
}
