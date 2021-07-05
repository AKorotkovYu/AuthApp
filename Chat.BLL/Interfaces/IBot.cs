namespace OneChat.BLL.Interfaces
{ 
    public interface IBot
    {
        public abstract string Name { get; }
        public abstract string Execute(string message);
    }
}
