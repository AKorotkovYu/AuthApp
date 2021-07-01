using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public string Nickname { get; set; }
        public bool isOld { get; set; }
        public DateTime TimeOfPosting { get; set; }
        public string Message { get; set; }
    }
}
