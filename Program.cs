using System;
using System.Linq;
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
            

        private static void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)// async?
        {
            var message = messageEventArgs.Message;
            var messageId = message.Chat.Id;
            if (!(AppSettings.BotUsers.ContainsKey(messageId)))
            {
                AppSettings.BotUsers.Add(messageId, new BotUser(messageId));
                AppSettings.Repository[message.Chat.Id] = new RepositoryGetter().GetRepository();
            }

            var currCondition = AppSettings.BotUsers[messageId].UserConditions.Last();
            if (message.Text == "/start")
            {
                currCondition = new StartCondition();
            }

            try
            {
                foreach (var command in currCondition.Commands)//////////////////////
                {
                    if (command.NeedToExecute(message))
                    {
                        command.Execute(message, message.Text);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                var user = AppSettings.BotUsers[message.Chat.Id];
                string lastCondition = user.UserConditions.Last().ToString();
                var userName = message.From.FirstName;
                Logger.WriteError(userName, lastCondition, ex.Message);
                MessageManager.MessageOutput(messageId, "Не знаю, что ты натворил, но не делай так больше -_- \nможешь продолжать использование");
            }
        }



            var messageId = callbackQuery.Message.Chat.Id;
            var message = callbackQuery.Message;
            var currCondition = AppSettings.BotUsers[messageId].UserConditions.Last();

            
            try
            {
                foreach (var command in currCondition.Commands)///////////////
                {
                    if (command.NeedToExecute(message))
                    {
                        command.Execute(message, callbackQuery.Data.Split('.')[1]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                var user = AppSettings.BotUsers[message.Chat.Id];
                string lastCondition = user.UserConditions.Last().ToString();
                var userName = message.From.FirstName;
                Logger.WriteError(userName, lastCondition, ex.Message);
                MessageManager.MessageOutput(messageId, "Не знаю, что ты натворил, но не делай так больше -_- \nможешь продолжать использование");
            }
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }
    }
}
