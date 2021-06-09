using Telegram.Bot.Types;

namespace kontur_project
{ 
    public abstract class Command
    {
        public static Bot ExecutorBot;
        public abstract void Execute(Message message, string text);
        public abstract bool NeedToExecute(Message message);
    }
}
