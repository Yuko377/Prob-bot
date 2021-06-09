using System;
using Telegram.Bot.Args;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace kontur_project
{
    public static class Program
    {
        public static async Task Main()
        {
            var myBot = new Bot();

            //var me = await myBot.botClient.GetMeAsync();
            //Console.Title = me.Username;


            //Bot.botClient.OnMessage += BotOnMessageReceived;
            //Bot.botClient.OnCallbackQuery += BotOnCallbackQueryReceived;

            //Bot.botClient.StartReceiving(Array.Empty<UpdateType>());
            //Console.WriteLine($"Start listening for @{me.Username}");

            //Console.ReadLine();
            //Bot.botClient.StopReceiving();
            Command.ExecutorBot = myBot;

            myBot.StartWork();
            Console.WriteLine($"Start listening for master");
            Console.ReadLine();
            myBot.StopWork();
            

        }





        #region Inline Mode

        //private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        //{
        //    Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

        //    InlineQueryResultBase[] results = {
        //        new InlineQueryResultArticle(
        //            id: "3",
        //            title: "TgBots",
        //            inputMessageContent: new InputTextMessageContent(
        //                "hello"
        //            )
        //        )
        //    };
        //    await Bot.botClient.AnswerInlineQueryAsync(
        //        inlineQueryId: inlineQueryEventArgs.InlineQuery.Id,
        //        results: results,
        //        isPersonal: true,
        //        cacheTime: 0
        //    );
        //}

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        #endregion

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }
    }
}
