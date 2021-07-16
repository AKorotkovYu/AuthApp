using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneChat.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int MessageActivity { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public DateTime LastMessageTime { get; set; }
        public float AverageMessageCountInDay { get; set; }
        public List<Chat> Chats { get; set; } = new List<Chat>();
    }
}
