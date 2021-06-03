using System;
using Telegram.Bot.Types;

namespace kontur_project
{
    public class DistributionReadingCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public void Execute(Message message, string key)// из словаря распределений из настроек берёт нужное по названию
        {
            if (!AppSettings.Repository[message.Chat.Id].ContainsKey(key))
            {
                MessageManager.MessageOutput(message.Chat.Id, "Выбери распределение из предложенных или введи его вручную");
                return;
            }
            var currType = AppSettings.Repository[message.Chat.Id][key];
            
            var ctor = currType.GetConstructor(new Type[] { });
            var currDistr = (Distribution)ctor.Invoke(new object[] { });
            var num = currType.GetProperty("ParamNum").GetValue(currDistr);
            AppSettings.BotUsers[message.Chat.Id].Distributions.Add(currDistr);

            MessageManager.MessageOutput(
                chatId: message.Chat.Id,
                text: $"Ты выбрал {key.ToLower()} распределение, введи {num} параметр(a) через пробел.");
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new DistributionParamsWaitingCondition());
        }

        public bool NeedToExecute(Message message)
        {
            if (message.Text == null)
            {
                MessageManager.MessageOutput(message.Chat.Id, "иди нахуй со своими стикерами аутист");
                return false;
            }
            return true;
        }
    }
}
