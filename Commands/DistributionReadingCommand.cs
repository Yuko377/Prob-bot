using System;
using Telegram.Bot.Types;

namespace kontur_project
{
    public class DistributionReadingCommand : Command
    {
        public override void Execute(Message message, string key)
        {
            var keys = AppSettings.Repository[message.Chat.Id].Keys;
            if (!MessageManager.IsItCorrect(key, keys))
            {
                ExecutorBot.SendTextMessage(message.Chat.Id, "Выбери распределение из предложенных или введи его вручную");
                return;
            }

            var currType = AppSettings.Repository[message.Chat.Id][key];
            
            var ctor = currType.GetConstructor(new Type[] { });
            var currDistr = (Distribution)ctor.Invoke(new object[] { });
            var num = currType.GetProperty("ParamNum").GetValue(currDistr);
            AppSettings.BotUsers[message.Chat.Id].Distributions.Add(currDistr);

            var text = $"Ты выбрал {key.ToLower()} распределение, введи {num} параметр(a) через пробел.";

            var currAttributes = currDistr.GetType().GetCustomAttributes(false);
            foreach (var attr in currAttributes)
            {
                if (attr.GetType() == typeof(ParametersDescriptionAttribute))
                {
                    text += "\n"+attr.ToString();
                    break;
                }
            }

            ExecutorBot.SendTextMessage(
                chatId: message.Chat.Id,
                text: text);
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Add(new DistributionParamsWaitingCondition());
        }

        public override bool NeedToExecute(Message message)
        {
            if (!MessageManager.IsItCorrect(message.Text))
            {
                ExecutorBot.SendTextMessage(message.Chat.Id, "распределение, а не стикер...");
                return false;
            }
            
            return true;
        }
    }
}
