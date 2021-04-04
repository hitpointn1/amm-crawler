using AMMCrawler.Providers;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMMCrawler.Abstractions
{
    interface ILinksProvider
    {
        Task<HashSet<LinkData>> GetLinksFromPage(IWebDriver driver, string selector);
    }
}
