using System.IO;

namespace kontur_project
{
    public static class FileManager
    {
        private readonly static string KeyFileName = "API_key.txt";
        private readonly static string DistributionsDirectoryName = "Distributions";
        private readonly static int UpLevelNumber = 3;
        
        internal static string GetAPIKeyPath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, $"Helping_files\\{KeyFileName}");

            return filePath;
        }

        internal static string GetDistributionsDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            for (var i = 0; i < UpLevelNumber; i++)
                currentDirectory = Path.GetDirectoryName(currentDirectory);

            var filePath = Path.Combine(currentDirectory, DistributionsDirectoryName);

            return filePath;
        }

        internal static string ReadFile(string fullPath)
        {
            using StreamReader sr = new StreamReader(fullPath);
            var text = sr.ReadToEnd();

            return text;
        }
    }
}
