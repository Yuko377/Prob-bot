using Telegram.Bot.Types;

namespace kontur_project
{ 
    public interface ICommand
    {
        public string Name { get; }
        public void Execute(Message message, string text);
        public bool NeedToExecute(Message message);
    }
}
