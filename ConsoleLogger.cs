using System;

namespace kontur_project
{
    public class ConsoleLogger: ILogger
    {
        public void StartLog()
        {
            Console.WriteLine("bot started working");
        }

        public void WriteError(long identificator, string condition, string message)
        {
            Console.WriteLine(
                $"Error:" +
                $"\nuser with this id:\t{identificator}" +
                $"\nwas in condition {condition}" +
                $"\nand broke the program with this message:\t{message}");
        }
    }
}
