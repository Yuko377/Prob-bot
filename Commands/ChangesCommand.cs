using Telegram.Bot.Types;

namespace kontur_project
{
    public class ChangesCommand : Command
    {
        private const string change = "changeMethod";
        private const string start = "ToStart";

        public override bool NeedToExecute(Message message)
        {
            var txt = message.Text;
            if (!MessageManager.IsMessageCorrect(message.Text))
            {
                ExecutorBot.SendTextMessage(message.Chat.Id, "смешно.");
                return false;
            }
            return true;
        }

        public override void Execute(Message message, string text)
        {
            if (!MessageManager.IsMessageCorrect(message.Text))
                ExecutorBot.SendTextMessage(message.Chat.Id, "Похоже, не то");

            if (text == change)
            {
                var tempCmd = new ParameterReadingCommand();
                tempCmd.Execute(message, text);

            }

            if (text == start)
            {
                var tempCmd = new StartCommand();
                tempCmd.Execute(message, text);

            }
        }
    }
}
