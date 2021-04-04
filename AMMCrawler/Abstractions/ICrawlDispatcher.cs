using AMMCrawler.ServiceLayer.DTO;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace AMMCrawler.Abstractions
{
    public interface ICrawlDispatcher
    {
        Task<int> PerformCrawl(IWebDriver driver, ResourceLinkDto linkDto, string clearUrl);
    }
}
