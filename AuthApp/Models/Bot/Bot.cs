using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models.Commands;

namespace AuthApp.Models.Bot
{
    public class Bot
    {
        private static List<Command> commandsList;

        public static List<Command> Commands => commandsList;

    }
}
