using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Services
{
    public interface IBot
    {
        public abstract string Name { get; }
        public abstract string Execute(string message);
    }
}
