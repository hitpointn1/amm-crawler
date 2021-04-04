using AMMCrawler.Core.Abstractions;
using System;
namespace AMMCrawler.Core
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(string message, Exception ex = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string logMessage = string.Format("{0} - {1}", DateTime.Now.ToString(), message);
            if (ex != null)
                logMessage = string.Format("{0} - Exception: {1}", logMessage, ex.ToString());
            Console.WriteLine(logMessage);
        }

        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string logMessage = string.Format("{0} - {1}", DateTime.Now.ToString(), message);
            Console.WriteLine(logMessage);
        }
    }
}
