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

            var currDistribution = AppSettings.BotUsers[currId].Distributions.Last();
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
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Add(new MethodWaitingCondition());
            var disrtibutionType = AppSettings.BotUsers[message.Chat.Id].Distributions.Last().GetType();
            var currMethods = disrtibutionType.GetMethods();
            var listForInlineKb = new List<List<InlineKeyboardButton>>();
            foreach(var method in currMethods)
            {
                var methodAttrs = method.GetCustomAttributes(true);// true чтобы учитывались аттрибуты классов-родителей
                var methodName = string.Empty;
                foreach(var attr in methodAttrs)
                {
                    if (attr.GetType() == typeof(NameMethodAttribute))
                        methodName = attr.ToString();
                }

                if(methodName != string.Empty)
                    listForInlineKb.Add(
                    new List<InlineKeyboardButton>()
                    {
                        InlineKeyboardButton.WithCallbackData(methodName, "method." + method.Name),
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
