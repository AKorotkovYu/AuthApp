using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.ViewModels
{
    public class ChatUserssModel
    {
        [Required(ErrorMessage = "Не указано имя Чата")]
        public string ChatName { get; set; }
        public string Nickname { get; set; }
    }
}
