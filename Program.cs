
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
            var messageId = message.Chat.Id;
            if (!(AppSettings.BotUsers.ContainsKey(messageId)))
            {
                AppSettings.BotUsers.Add(messageId, new BotUser(messageId));
            }

            var currCondition = AppSettings.BotUsers[messageId].UserConditions.Peek();

            foreach (var command in currCondition.Commands)
            {
                if (command.NeedToExecute(message))
                {
                    await command.Execute(message, message.Text);
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

            var messageId = callbackQuery.Message.Chat.Id;
            var message = callbackQuery.Message;
            var currCondition = AppSettings.BotUsers[messageId].UserConditions.Peek();

            foreach (var command in currCondition.Commands)
            {
                if (command.NeedToExecute(message))
                {
                    await command.Execute(message, callbackQuery.Data.Split('.')[1]);
                    break;
                }
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



