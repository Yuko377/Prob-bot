using System;
using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections;
using System.Globalization;

namespace kontur_project
{

    public interface ICommand
    {
        public string Name { get; }

        public void Execute(Message message, string text);

        public bool NeedToExecute(Message message);
    }

    public class StartCommand : ICommand
    {
        public string Name => @"/start";

        public bool NeedToExecute(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text )
                return false;

            return message.Text.Contains(this.Name);
        }

        public void Execute(Message message, string text)
        {
            SendDistributionKeyboard(message, AppSettings.Repository);
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new DistributionWaitingCondition());
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
            MessageManager.MessageOutput(
                chatId: message.Chat.Id,
                text: "Выбери распределение",
                replyMarkup: distributionKeyboard);
        }
    }

    public class DistributionReadingCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public void Execute(Message message, string key)
        {
            var currType = AppSettings.Repository[key];
            var ctor = currType.GetConstructor(new Type[] { });
            var currDistr = (IDistribution)ctor.Invoke(new object[] { });
            var num = currType.GetProperty("ParamNum").GetValue(currDistr);
            AppSettings.BotUsers[message.Chat.Id].Distribution.Add(currDistr);


            MessageManager.MessageOutput(
                chatId: message.Chat.Id,
                text: $"Ты выбрал {key.ToLower()} распределение, введи {num} параметр(a) в порядке возрастания. Дробная часть через запятую, числа через пробел"
            );
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new DistributionParamsWaitingCondition());
        }

        public bool NeedToExecute(Message message)
        {
            return true;
        }
    }

    public class ParameterReadingCommand : ICommand
    {
        public string Name => "param_cmd";

        public bool NeedToExecute(Message message)
        {
            var currId = message.Chat.Id;
            var currParams = message.Text.Split(' ').Where(x => x != "").ToArray();
            if (currParams.Length != AppSettings.BotUsers[currId].Distribution.Last().ParamNum)
            {
                MessageManager.MessageOutput(
                    chatId: currId,
                    text: "Много параметров, попробуй еще раз");// много параметров

                return false;
            }
            double doubleParameter;
            var doubleParams = new List<double>(currParams.Length);

            foreach (var el in currParams)
            {
                var el1 = el.Replace(',', '.');
                if (!double.TryParse(el1, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleParameter))
                {
                    Console.WriteLine("не парсится");
                    MessageManager.MessageOutput(
                        chatId: currId,
                        text: "Параметры не распознаны, попробуй еще раз");

                    return false;
                }

                doubleParams.Add(doubleParameter);
            }


            if (doubleParams.Count == 2)
            {
                if (doubleParams[0] >= doubleParams[1])
                {
                    MessageManager.MessageOutput(
                        chatId: message.Chat.Id,
                        text: "Что-то не так с параметрами, попробуй еще раз");
                    return false;
                }
            }

            AppSettings.BotUsers[currId].Distribution.Last().distParams = doubleParams;
            return true;

        }

        public void Execute(Message message, string text)
        {
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new MethodWaitingCondition());
            var currMethods = AppSettings.AvailableMethods;
            var listForInlineKb = new List<List<InlineKeyboardButton>>();
            foreach (var method in currMethods.Keys)
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

    public class MethodReadingCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public bool NeedToExecute(Message message)
        { 
            return true;
        }

        public void Execute(Message message, string methodName)
        {
            AppSettings.BotUsers[message.Chat.Id].Methods.Add(methodName);
            AppSettings.BotUsers[message.Chat.Id].UserConditions.Push(new MethodArgsWaitingCondition());
            MessageManager.MessageOutput(
                chatId: message.Chat.Id,
                text: "Вбей аргумент");

        }
    }

    public class MethodArgsWaitingCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public bool NeedToExecute(Message message)
        {
            if (message.ReplyMarkup != null)
                return false;

            var currParams = message.Text.Split(' ').Where(x => x != "").ToArray();
            if (currParams.Length != 1)
            {
                MessageManager.MessageOutput(
                    chatId: message.Chat.Id,
                    text: "Что-то не так с аргументами, попробуй еще раз");
                return false;
            }

            double doubleParameter;
            var inputParam = currParams[0];
            inputParam = inputParam.Replace(',', '.');

            if (!double.TryParse(inputParam, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleParameter))
            {
                Console.WriteLine("не парсится");
                MessageManager.MessageOutput(
                    chatId: message.Chat.Id,
                    text: "Что-то не так с параметрами, попробуй еще раз");

                return false;
            }

            AppSettings.BotUsers[message.Chat.Id].Args.Add(doubleParameter);

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

    public class ChangesCommand : ICommand
    {
        public string Name => throw new NotImplementedException();

        public bool NeedToExecute(Message message)
        {
            return true;
        }

        public void Execute(Message message, string text)
        {
            if (text == "changeMethod")
            {
                var tempCmd = new ParameterReadingCommand();
                tempCmd.Execute(message, text);

            }

            if (text == "ToStart")
            {
                var tempCmd = new StartCommand();
                tempCmd.Execute(message, text);

            }
        }
    }
}
