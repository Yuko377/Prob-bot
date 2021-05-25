using System.IO;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public static class MessageManager
    {
        public static string GetMessage(string fileName)// возможно есть смысл сделать асинхронным
        {
            var buff = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(buff, $"Helping_files\\{fileName}.txt");

            using StreamReader sr = new StreamReader(filePath);
            var message = sr.ReadToEnd();
            
            return message;
        }

        public static async void MessageOutput(
            ChatId chatId, string text, ParseMode parseMode = ParseMode.Default,
            bool disableWebPagePreview = false, bool disableNotification = false,
            int replyToMessageId = 0, IReplyMarkup replyMarkup = null,
            CancellationToken cancellationToken = default)
        {
            await Bot.botClient.SendTextMessageAsync(
                chatId, text, parseMode,disableWebPagePreview, disableNotification,
                replyToMessageId, replyMarkup, cancellationToken);
        }
    }
}
