using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace kontur_project
{
    public static class AppSettings
    {
        public static Dictionary<string, Type> Repository = new RepositoryGetter().GetRepository();

        public static Dictionary<long, BotUser> BotUsers = new Dictionary<long, BotUser>();
        public static string Name { get; set; } = "<BOT_NAME>";
        public static string Key { get; } = MessageManager.GetMessage("API_key");
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

            var repository = new Dictionary<string, Type>();

            foreach(var textDistribution in textDistributions)
            {
                var script = CSharpScript.Create(textDistribution, ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                var distType = (Type)script.RunAsync().Result.ReturnValue;

                var constr = distType.GetConstructor(new Type[] { });
                var currDist = (Distribution)constr.Invoke(new object[] { });
                var distName = currDist.Name;

                repository.Add(distName, distType);
            }

            return repository;
        }
    }
}
