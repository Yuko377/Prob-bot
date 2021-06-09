﻿using System;
using Telegram.Bot.Types;

namespace kontur_project
{
    public class ChangesCommand : Command
    {
        public override bool NeedToExecute(Message message)
        {
            var txt = message.Text;
            if (!MessageManager.IsItCorrect(message.Text))
            {
                ExecutorBot.SendTextMessage(message.Chat.Id, "смешно.");
                return false;
            }
            return true;
        }

        public override void Execute(Message message, string text)
        {
            if (!MessageManager.IsItCorrect(message.Text))
                ExecutorBot.SendTextMessage(message.Chat.Id, "Похоже, не то");

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
