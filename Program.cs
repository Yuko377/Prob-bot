
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public static class Program
    {
        public static async Task Main()
        {
            var myBot = new Bot();

            var me = await Bot.botClient.GetMeAsync();
            Console.Title = me.Username;


            Bot.botClient.OnMessage += BotOnMessageReceived;
            Bot.botClient.OnCallbackQuery += BotOnCallbackQueryReceived;

            Bot.botClient.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");

            Console.ReadLine();
            Bot.botClient.StopReceiving();
        }


        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {

            var message = messageEventArgs.Message;
            foreach (var command in Bot.commandsList)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message);
                    break;
                }
            }
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await Bot.botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}"
            );
            var queryType = callbackQuery.Data.Split('.').First();
            if (queryType == "distribution")
            {
                var key = callbackQuery.Data.Split('.')[1];
                var currType = AppSettings.Repository[key];
                var ctor = currType.GetConstructor(new Type[] { });
                var currDistr = (IDistribution)ctor.Invoke(new object[] { });
                var num = currType.GetProperty("ParamNum").GetValue(currDistr);
                if (!(AppSettings.MyDistributions.TryAdd(callbackQuery.Message.Chat.Id, currDistr)))
                {
                    AppSettings.MyDistributions.Remove(callbackQuery.Message.Chat.Id);
                    AppSettings.MyDistributions.Add(callbackQuery.Message.Chat.Id, currDistr);
                }
                    
                
                await Bot.botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: $"Ты выбрал {key.ToLower()} распределение, введи {num} параметр(a). Дробная часть через запятую, числа через пробел"
                );
                
            }

            if (queryType == "method")
            {
                var currId = callbackQuery.Message.Chat.Id;
                var methodName = callbackQuery.Data.Split('.')[1];
                var currMethod = AppSettings.MyDistributions[currId].GetType().GetMethod(methodName);
                var result = currMethod.Invoke(AppSettings.MyDistributions[currId], new Object[]{});
                await Bot.botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: result.ToString()
                );
            }

            
        }

        #region Inline Mode

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };
            await Bot.botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQueryEventArgs.InlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0
            );
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        #endregion

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }

        
    }
}



