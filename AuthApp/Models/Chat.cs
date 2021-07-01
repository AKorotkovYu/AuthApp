using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Models
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
