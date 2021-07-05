using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneChat.WEB.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string ChatName { get; set; }
        public int AdminId { get; set; }
    }
}
