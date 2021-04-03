using AMMCrawler.Entities;
using System;
using System.Threading.Tasks;

namespace AMMCrawler.Abstractions
{
    internal interface ICrawler : IDisposable
    {
        Task Crawl(string url);
        Task<ResourceLink> GetAvailableLink();
    }
}
