using System;
using System.IO;

namespace kontur_project
{
    public class MessageManager
    {
        private readonly string fileName;
        
        public MessageManager(string fileName)
        {
            this.fileName = fileName;
        }

        public string GetMessage()// возможно есть смысл сделать асинхронным
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Help\\{fileName}.txt");

            var message = string.Empty;
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            try
            {
                using StreamReader sr = new StreamReader(filePath);
                message = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return message;
        }
    }
}
