using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneChat.DAL.Entities;

namespace OneChat.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public List<Chat> Chats { get; set; } = new List<Chat>();
    }
}
