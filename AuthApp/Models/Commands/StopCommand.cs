using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models.Bot;

namespace AuthApp.Models.Commands
{
    public class StopCommand: Command
    {
        public override string Name => @"/stop";

        public override string Execute(string message)
        {
            string answer = string.Empty;

            /*логика*/

            return "end";
        }
    }
}
