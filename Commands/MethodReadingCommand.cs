using System;
using System.Linq;
using Telegram.Bot.Types;

namespace kontur_project
{
    public class MethodReadingCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public bool NeedToExecute(Message message)
        {
            if (!MessageManager.IsItCorrect(message.Text))
            {
                MessageManager.MessageOutput(message.Chat.Id, "Я просил метод, а не стикер :)");
                return false;
            }

            return true;
        }

        public void Execute(Message message, string methodName)//methodName это то, что ввёл пользователь
        {
            var currMethod = AppSettings.BotUsers[message.Chat.Id].Distributions.Last().GetType().GetMethod(methodName);

            int argNum = currMethod.GetParameters().Length;

            if (argNum == 0)
            {
                AppSettings.BotUsers[message.Chat.Id].Methods.Add(methodName);
                AppSettings.BotUsers[message.Chat.Id].Args.Add(new double[] { });
                AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new MethodArgsWaitingCondition());
                var tempCmd = new MethodArgsWaitingCommand();
                tempCmd.Execute(message, "owo");
            }
            else
            {
                AppSettings.BotUsers[message.Chat.Id].Methods.Add(methodName);
                AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new MethodArgsWaitingCondition());
                MessageManager.MessageOutput(
                    chatId: message.Chat.Id,
                    text: "Вбей аргумент");
            }
            
        }
    }
}
