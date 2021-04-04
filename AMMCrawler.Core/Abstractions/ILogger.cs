using System;

namespace AMMCrawler.Core.Abstractions
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message, Exception ex = null);
    }
}
