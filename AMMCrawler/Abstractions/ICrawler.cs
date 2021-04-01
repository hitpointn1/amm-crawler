using AMMCrawler.Entities;
using System.Threading.Tasks;

namespace AMMCrawler.Abstractions
{
    internal interface ICrawler
    {
        Task Crawl(string url);
        Task<ResourceLink> GetAvailableLink();
    }
}
