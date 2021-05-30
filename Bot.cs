using Telegram.Bot;


namespace kontur_project
{
    public class Bot
    {
        public static TelegramBotClient botClient;

        public Bot()
        {
            botClient = new TelegramBotClient(AppSettings.Key);
        }

    }
}
