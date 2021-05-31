using System;
using System.Collections.Generic;

namespace kontur_project
{
    public static class AppSettings
    {
        public static Dictionary<string, Type> Repository = new Dictionary<string, Type>
        {
            {"Равномерное", typeof(UniformDistribution) },
            {"Показательное", typeof(ExpDistribution) }
        };

        public static Dictionary<long, BotUser> BotUsers = new Dictionary<long, BotUser>();
        public static string Name { get; set; } = "<BOT_NAME>";
        public static string Key { get; set; } = MessageManager.GetMessage("API_key");
    }
}
