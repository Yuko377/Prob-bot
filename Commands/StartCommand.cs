using System;
using Telegram.Bot.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public class StartCommand : Command
    {
        public string Name => @"/start";

        public override bool NeedToExecute(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            if (!message.Text.Contains("/start"))
            {
                ExecutorBot.SendTextMessage(message.Chat.Id, "Привет, "+ message.From.FirstName+ "! Введи /start для начала работы");
            }

            return message.Text.Contains(this.Name);
        }

        public override void Execute(Message message, string text)
        {
            SendDistributionKeyboard(message, new RepositoryGetter().GetRepository());
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Add(new DistributionWaitingCondition());
        }

        static void SendDistributionKeyboard(Message message, Dictionary<string, Type> repository)
        {
            var listForInlineKb = new List<List<InlineKeyboardButton>>();
            foreach (var key in repository.Keys)
            {
                listForInlineKb.Add(
                    new List<InlineKeyboardButton>()
                    {
                        InlineKeyboardButton.WithCallbackData(key, "distribution." + key),
                    });
            }

            var distributionKeyboard = new InlineKeyboardMarkup(listForInlineKb);

            ExecutorBot.SendTextMessage(message.Chat.Id, "Выбери распределение", distributionKeyboard);
        }
    }
}
