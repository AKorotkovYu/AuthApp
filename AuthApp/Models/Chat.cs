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
        public DbSet <ChatMessages> chatMessages
    }
}
