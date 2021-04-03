using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMMCrawler.Abstractions
{
    interface ILinksProvider
    {
        Task<HashSet<string>> GetLinksFromPage(IWebDriver driver, string selector);
    }
}
