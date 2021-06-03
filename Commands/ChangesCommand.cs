using System;
using Telegram.Bot.Types;

namespace kontur_project
{
    public class ChangesCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public bool NeedToExecute(Message message)
        {
            var txt = message.Text;
            if (!MessageManager.IsItCorrect(message.Text))
            {
                MessageManager.MessageOutput(message.Chat.Id, "смешно.");
                return false;
            }
            return true;
        }

        public void Execute(Message message, string text)
        {
            if (!MessageManager.IsItCorrect(message.Text))
                MessageManager.MessageOutput(message.Chat.Id, "Похоже, не то");

            if (text == "changeMethod")
            {
                var tempCmd = new ParameterReadingCommand();
                tempCmd.Execute(message, text);

            }

            if (text == "ToStart")
            {
                var tempCmd = new StartCommand();
                tempCmd.Execute(message, text);

            }
        }
    }
}
