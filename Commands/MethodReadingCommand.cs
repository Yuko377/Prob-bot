using System;
using Telegram.Bot.Types;

namespace kontur_project
{
    public class MethodReadingCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public bool NeedToExecute(Message message)
        {
            return true;
        }

        public void Execute(Message message, string methodName)
        {
            AppSettings.BotUsers[message.Chat.Id].Methods.Add(methodName);
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new MethodArgsWaitingCondition());
            MessageManager.MessageOutput(
                chatId: message.Chat.Id,
                text: "Вбей аргумент");
        }
    }
}
