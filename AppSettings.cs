using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace kontur_project
{
    public static class AppSettings
    {
        public static Dictionary<string, Type> Repository = new Dictionary<string, Type>
        {
            {"Равномерное", typeof(UniformDistribution) },
            {"Показательное", typeof(ExpDistribution) },
            {"Нормальное", typeof(NormalDistribution) },
            {"Коши", typeof(CauchyDistribution) }
        };

        public static Dictionary<long, BotUser> BotUsers = new Dictionary<long, BotUser>();
        public static string Name { get; set; } = "<BOT_NAME>";
        public static string Key { get; set; } = MessageManager.GetMessage("API_key");
    }
    
    internal static class DirectoryWalker
    {
        internal static string GetDistributionsDirectory()
        {
            var cerrentDirectory = Directory.GetCurrentDirectory();

            for(var i = 0; i < 3; i++)
                cerrentDirectory = Path.GetDirectoryName(cerrentDirectory);
            
            var filePath = Path.Combine(cerrentDirectory, "Distributions");

            return filePath;
        }

        internal static string[] GetDistributionsInside(string path)
        {
            var filesNames = Directory.GetFiles(path);

            var asw = new List<string>();

            foreach(var fileName in filesNames)
            {
                using StreamReader sr = new StreamReader(fileName);
                var text = sr.ReadToEnd();
                var text += 
                asw.Add(text);
            }

            return asw.ToArray();
        }
    }

    internal class RepositoryGetter
    {
        internal Dictionary<string, Type> GetRepository()
        {
            var distributionsDirectory = DirectoryWalker.GetDistributionsDirectory();
            var textDistributions = DirectoryWalker.GetDistributionsInside(distributionsDirectory);

        }
    }
}
