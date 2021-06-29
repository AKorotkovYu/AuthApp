using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models.Bot;

namespace AuthApp.Models.Commands
{
    public class StartCommand: Command
    {
        public override string Name => @"/start";

        public override string Execute(string message)
        {
            string answer = string.Empty;

            /*логика*/

            return "start";
        }
    }
}
