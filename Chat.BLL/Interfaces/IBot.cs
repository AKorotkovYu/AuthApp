using System.Threading.Tasks;

namespace OneChat.BLL.Interfaces
{
    public interface IBot
    {
        public abstract string Name { get; }
        public abstract Task<string> ExecuteAsync(string message);

    }
}
