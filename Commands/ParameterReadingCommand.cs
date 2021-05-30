using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public class ParameterReadingCommand : ICommand
    {
        public string Name => "param_cmd";

        public bool NeedToExecute(Message message)
        {
            var currId = message.Chat.Id;
            var currParams = InputsParser.Parse(message.Text);
            if (currParams == null)
            {
                MessageManager.MessageOutput(
                    chatId: currId,
                    text: "Не удалось распознать параметры, попробуй еще раз");

                return false;
            }

            var currDistribution = AppSettings.BotUsers[currId].Distribution.Last();
            var paramsTrubles = currDistribution.CheckParamsValid(currParams);
            if (paramsTrubles != null)
            {
                MessageManager.MessageOutput(
                    chatId: currId,
                    text: paramsTrubles);

                return false;
            }

            currDistribution.distParams = currParams;
            return true;
        }

        public void Execute(Message message, string text)
        {
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new MethodWaitingCondition());
            var currMethods = AppSettings.AvailableMethods;
            var listForInlineKb = new List<List<InlineKeyboardButton>>();
            foreach (var method in currMethods.Keys)//по названиям методов
            {
                listForInlineKb.Add(
                    new List<InlineKeyboardButton>()
                    {
                        InlineKeyboardButton.WithCallbackData(method, "method." + currMethods[method]),
                    });
            }

            var methodsKeyboard = new InlineKeyboardMarkup(listForInlineKb);
            MessageManager.MessageOutput(
                chatId: message.Chat.Id,
                text: "Выбери метод",
                replyMarkup: methodsKeyboard);
        }
    }
}
