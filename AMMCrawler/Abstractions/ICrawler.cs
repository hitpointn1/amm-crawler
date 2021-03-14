using System.Threading.Tasks;

namespace AMMCrawler.Abstractions
{
    internal interface ICrawler
    {
        Task Crawl();
    }
}
