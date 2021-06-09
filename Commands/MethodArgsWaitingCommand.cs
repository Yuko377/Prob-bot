using System;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public class MethodArgsWaitingCommand : Command
    {
        public override bool NeedToExecute(Message message)
        {
            if (message.ReplyMarkup != null)
                return false;
             
            var currId = message.Chat.Id;
            var methodName = AppSettings.BotUsers[currId].Methods.Last();
            var currMethod = AppSettings.BotUsers[currId].Distributions.Last().GetType().GetMethod(methodName);
            int argNum = currMethod.GetParameters().Length;

            if(argNum == 0)
            {
                AppSettings.BotUsers[message.Chat.Id].Args.Add(new double[] { });
                return true;
            }

            var currArgs = InputsParser.Parse(message.Text);
            if (currArgs == null)
            {
                ExecutorBot.SendTextMessage(
                    chatId: currId,
                    text: "Не удалось распознать параметры, попробуй еще раз");

                return false;
            }

            if (currArgs.Length != argNum)
            {
                ExecutorBot.SendTextMessage(
                    chatId: currId,
                    text: "Число параметров не соответствует методу");

                return false;
            }

            AppSettings.BotUsers[message.Chat.Id].Args.Add(currArgs);

            return true;
        }

        public override void Execute(Message message, string text)
        {
            long currId = message.Chat.Id;

            string methodName = AppSettings.BotUsers[currId].Methods.Last();
            MethodInfo currMethod = AppSettings.BotUsers[currId].Distributions.Last().GetType().GetMethod(methodName);
            var args = AppSettings.BotUsers[currId].Args
                .Last()
                .Select(d => (object)d)
                .ToArray();
            Distribution obj = AppSettings.BotUsers[currId].Distributions.Last();

            var result = currMethod.Invoke(obj, args);
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

            //ExecutorBot.SendTextMessage(
            //    chatId: currId,
            //    text: result.ToString(),
            //    replyMarkup: changesKeyboard);

            ExecutorBot.SendTextMessage(currId, result.ToString(), changesKeyboard);
        }
    }
}
