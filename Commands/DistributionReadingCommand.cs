using System;
using Telegram.Bot.Types;

namespace kontur_project
{
    public class DistributionReadingCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public void Execute(Message message, string key)// из словаря распределений из настроек берёт нужное по названию
        {
            var currType = AppSettings.Repository[key];
            var ctor = currType.GetConstructor(new Type[] { });
            var currDistr = (Distribution)ctor.Invoke(new object[] { });
            var num = currType.GetProperty("ParamNum").GetValue(currDistr);
            AppSettings.BotUsers[message.Chat.Id].Distribution.Add(currDistr);

            MessageManager.MessageOutput(
                chatId: message.Chat.Id,
                text: $"Ты выбрал {key.ToLower()} распределение, введи {num} параметр(a) через пробел.");
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new DistributionParamsWaitingCondition());
        }

        public bool NeedToExecute(Message message)
        {
            return true;
        }
    }
}
