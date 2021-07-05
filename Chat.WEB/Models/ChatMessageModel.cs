using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneChat.WEB.Models
{
    public class ChatMessageModel
    {
        public int Id { get; set; }
        public int ChatId { get;  set; }
        public string ChatName { get; set; }
        public string Nickname { get; set; }
        public bool IsOld { get; set; }
        public DateTime TimeOfPosting { get; set; }
        public string Message { get; set; }
    }
}
