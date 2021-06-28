using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Models
{
    public class ChatUsers
    {
        public int Id { get; set; }
        public string ChatName { get; set; }
        public string Nickname { get; set; }
        public DateTime DateofAdding { get; set; }
    }
}
