using System;

namespace kontur_project
{
    internal static class Logger
    {
        internal static void StartLog()
        {
            Console.WriteLine("bot started working");
        }

        internal static void WriteError(long identificator, string condition, string message)
        {
            Console.WriteLine(
                $"Error:" +
                $"\nuser with this id:\t{identificator}" +
                $"\nwas in condition {condition}" +
                $"\nand broke the program with this message:\t{message}");
        }
    }
}
