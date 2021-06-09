using Telegram.Bot.Types;

namespace kontur_project
{ 
    public interface ICommand
    {
        public string Name { get; }
        public void Execute(Message message, string text);
        public bool NeedToExecute(Message message);

    }

    public abstract class Command
    {
        public static Bot ExecutorBot;
        public abstract void Execute(Message message, string text);
        public abstract bool NeedToExecute(Message message);
    }
}
