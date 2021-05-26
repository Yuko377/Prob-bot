using System;
using Telegram.Bot.Types;

namespace kontur_project
{
    public class ChangesCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public bool NeedToExecute(Message message)
        {
            return true;
        }

        public void Execute(Message message, string text)
        {
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
