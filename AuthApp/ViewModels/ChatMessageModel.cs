using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.ViewModels
{
    public class ChatMessageModel
    {
        public string ChatName { get; set; }
        public string Nickname { get; set; }
        public DateTime TimeOfPosting { get; set; }
        public string Message { get; set; }
    }
}
