using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public class ParameterReadingCommand : Command
    {
        public string Name => "param_cmd";

        public override bool NeedToExecute(Message message)
        {
            var currId = message.Chat.Id;
            var currParams = InputsParser.Parse(message.Text);
            if (currParams == null)
            {
                ExecutorBot.SendTextMessage(
                    chatId: currId,
                    text: "Не удалось распознать параметры, попробуй еще раз");

                return false;
            }

            var currDistribution = AppSettings.BotUsers[currId].Distributions.Last();
            var paramsTroubles = currDistribution.CheckParamsValid(currParams);
            if (paramsTroubles != null)
            {
                ExecutorBot.SendTextMessage(
                    chatId: currId,
                    text: paramsTroubles);

                return false;
            }

            currDistribution.distParams = currParams;
            return true;
        }
        
        public override void Execute(Message message, string text)
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
            //ExecutorBot.SendTextMessage(
            //    chatId: message.Chat.Id,
            //    text: "Выбери метод",
            //    replyMarkup: methodsKeyboard);

            ExecutorBot.SendTextMessage(message.Chat.Id, "Выбери метод", methodsKeyboard);
        }
    }
}
