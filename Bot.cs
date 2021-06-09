using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


namespace kontur_project
{
    public class Bot
    {
        private const string startCommand = "/start";

        public TelegramBotClient botClient;
        public string botName;


        public Bot()
        {
            botClient = new TelegramBotClient(AppSettings.Key);
            this.botClient.OnMessage += BotOnMessageReceived;
            this.botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
        }

        public void StartWork()
        {
            this.botClient.StartReceiving();
        }

        public void StopWork()
        {
            this.botClient.StopReceiving();
        }

        public async void SendTextMessage(ChatId chatId, string text, IReplyMarkup myReplyMarkup = null)
        {
           await this.botClient.SendTextMessageAsync(chatId, text, replyMarkup: myReplyMarkup);
        }

        private void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            var messageId = message.Chat.Id;
            if (!AppSettings.BotUsers.ContainsKey(messageId))
            {
                AppSettings.BotUsers.Add(messageId, new BotUser(messageId));
                AppSettings.Repository[message.Chat.Id] = new RepositoryGetter().GetRepository();
            }

            var currCondition = AppSettings.BotUsers[messageId].UserConditions.Last();
            if (message.Text == startCommand)
            {
                currCondition = new StartCondition();
            }

            try
            {
                foreach (var command in currCondition.Commands)
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
                var userId = message.Chat.Id;
                Logger.WriteError(userId, lastCondition, ex.Message);
                this.SendTextMessage(messageId, "Не знаю, что ты натворил, но не делай так больше -_- \nможешь продолжать использование");
            }
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await this.botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            var messageId = callbackQuery.Message.Chat.Id;
            var message = callbackQuery.Message;
            var currCondition = AppSettings.BotUsers[messageId].UserConditions.Last();


            try
            {
                foreach (var command in currCondition.Commands)
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
                var userId = message.Chat.Id;
                Logger.WriteError(userId, lastCondition, ex.Message);
                this.SendTextMessage(messageId, "Не знаю, что ты натворил, но не делай так больше -_- \nможешь продолжать использование");
            }
        }


    }
}
