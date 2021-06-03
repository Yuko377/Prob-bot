using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace kontur_project
{
    public static class AppSettings
    {
        public static Dictionary<long, Dictionary<string, Type>> Repository = new ();

        public static Dictionary<long, BotUser> BotUsers = new Dictionary<long, BotUser>();
        public static string Name { get; set; } = "<BOT_NAME>";
        public static string Key { get; } = MessageManager.GetMessage("API_key");
    }
    
    internal class DirectoryWalker
    {
        internal static string GetDistributionsDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            for(var i = 0; i < 3; i++)
                currentDirectory = Path.GetDirectoryName(currentDirectory);
            
            var filePath = Path.Combine(currentDirectory, "Distributions");

            return filePath;
        }

        internal string[] GetDistributionsInside(string path)
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
            var textDistributions = new DirectoryWalker().GetDistributionsInside(distributionsDirectory);

            var repository = new Dictionary<string, Type>();

            foreach(var textDistribution in textDistributions)
            {
                var path = Assembly.GetAssembly(typeof(Distribution)).Location;
                var asm = AssemblyMetadata.CreateFromFile(path).GetReference();

                var options = ScriptOptions.Default.AddImports("System", "kontur_project").AddReferences(asm);
                var script = CSharpScript.Create(textDistribution, options);
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
