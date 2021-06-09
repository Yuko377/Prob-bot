using Telegram.Bot.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace kontur_project
{
    public static class MessageManager
    {
        public static bool IsItCorrect(string text, ICollection<string> map = null )
        {
            if (map != null)
                return map.Contains(text);

            return text != null;
        }

        public static async void MessageOutput(ChatId chatId, string text, InlineKeyboardMarkup replyMarkup=default)
        {
            await Bot.botClient.SendTextMessageAsync(chatId, text, replyMarkup: replyMarkup);
        }
    }
}
