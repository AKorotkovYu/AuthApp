using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OneChat.WEB.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string ChatName { get; set; }

        [Required]
        public int AdminId { get; set; }
    }
}
