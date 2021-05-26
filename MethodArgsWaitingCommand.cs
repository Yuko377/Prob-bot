using System;
using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public class MethodArgsWaitingCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public bool NeedToExecute(Message message)
        {
            if (message.ReplyMarkup != null)
                return false;

            var currId = message.Chat.Id;
            var currArgs = InputsParser.Parse(message.Text);
            if (currArgs == null)
            {
                MessageManager.MessageOutput(
                    chatId: currId,
                    text: "Не удалось распознать параметры, попробуй еще раз");

                return false;
            }

            var methodName = AppSettings.BotUsers[currId].Methods.Last();
            var currMethod = AppSettings.BotUsers[currId].Distribution.Last().GetType().GetMethod(methodName);
            int argNum = currMethod.GetParameters().Length;
            if (currArgs.Length != argNum)
            {
                MessageManager.MessageOutput(
                    chatId: currId,
                    text: "Число параметров не соответствует методу");

                return false;
            }

            AppSettings.BotUsers[message.Chat.Id].Args.Add(currArgs[0]);

            return true;
        }

        public void Execute(Message message, string text)
        {
            var currId = message.Chat.Id;
            var methodName = AppSettings.BotUsers[currId].Methods.Last();
            var currMethod = AppSettings.BotUsers[currId].Distribution.Last().GetType().GetMethod(methodName);
            var result = currMethod.Invoke(AppSettings.BotUsers[currId].Distribution.Last(),
                                                    new object[] { AppSettings.BotUsers[currId].Args.Last() });

            var listForInlineKb = new List<List<InlineKeyboardButton>>();

            listForInlineKb.Add(
                new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData("Выбрать другой метод", "change.changeMethod"),
                });
            listForInlineKb.Add(
                new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData("В начало", "change.ToStart"),
                });

            var changesKeyboard = new InlineKeyboardMarkup(listForInlineKb);

            MessageManager.MessageOutput(
                chatId: currId,
                text: result.ToString(),
                replyMarkup: changesKeyboard);
        }
    }
}
