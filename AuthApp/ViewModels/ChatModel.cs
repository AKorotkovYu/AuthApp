using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models;

namespace AuthApp.ViewModels
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string ChatName { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<User> ChatUsers { get; set; } = new List<User>();
        public int AdminId { get; set; }
    }
}
