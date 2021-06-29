using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Models.Bot
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract string Execute(string message);
    }
}
