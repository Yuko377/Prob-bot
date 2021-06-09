using System;

namespace kontur_project
{
    public static class Program
    {
        public static void Main()
        {
            var myBot = new Bot();

            Command.ExecutorBot = myBot;

            myBot.StartWork();
            Console.WriteLine($"Start listening for master");
            Console.ReadLine();
            myBot.StopWork();
        }
    }
}
