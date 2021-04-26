using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public interface ICommand
    {
        public string Name { get; }

        public Task Execute(Message message);

        public bool Contains(Message message);
    }

    public class StartCommand : ICommand
    {

        public string Name => @"/start";

        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text )
                return false;

            return message.Text.Contains(this.Name);
        }

        public async Task Execute(Message message)
        {
            await SendDistributionKeyboard(message, AppSettings.Repository);
        }

        static async Task SendDistributionKeyboard(Message message, Dictionary<string, Type> repository)
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
            await Bot.botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Выбери распределение",
                replyMarkup: distributionKeyboard
            );

        }
    }

    public class ParameterCommand : ICommand
    {
        public string Name => "param_cmd";

        public bool Contains(Message message)
        {
            var currId = message.Chat.Id;
            var currParams = message.Text.Split(' ');
            //Console.WriteLine(currParams[0]);
            if (!AppSettings.MyDistributions.ContainsKey(currId))
            {
                 Bot.botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Сначала ввел параметры, емае"
                );
              //   Bot.botClient.DeleteMessageAsync(message.Chat.Id);

            }
            Console.WriteLine(currParams.Length);
            Console.WriteLine(AppSettings.MyDistributions[currId].ParamNum);
            if (currParams.Length != AppSettings.MyDistributions[currId].ParamNum)
            {
                Bot.botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Что-то не так с параметрами, попробуй еще раз");
                return false;
            }
            double doubleParameter;
            var doubleParams = new List<double>(currParams.Length);
            foreach (var el in currParams)
            {
                if (!double.TryParse(el, out doubleParameter))
                {
                    Console.WriteLine("не парсится");
                    Bot.botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Что-то не так с параметрами, попробуй еще раз");
                    return false;

                }
                doubleParams.Add(doubleParameter);
            }

            AppSettings.MyDistributions[currId].distParams = doubleParams;
            return true;

        }

        public async Task Execute(Message message)
        {
            if (Contains(message))
            {
                var currMethods = AppSettings.MyDistributions[message.Chat.Id].
                    GetType().
                    GetMethods().Where(method => method.Name.Contains("Method"));
                var listForInlineKb = new List<List<InlineKeyboardButton>>();
                foreach (var method in currMethods)
                {
                    listForInlineKb.Add(
                        new List<InlineKeyboardButton>()
                        {
                            InlineKeyboardButton.WithCallbackData(method.Name, "method." + method.Name),
                        });
                }

                var methodsKeyboard = new InlineKeyboardMarkup(listForInlineKb);
                await Bot.botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Выбери метод",
                    replyMarkup: methodsKeyboard
                );

            }

            if (!Contains(message))
            {
                await Bot.botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Что-то не так с параметрами"
                );
            }

            
        }
    }
}
