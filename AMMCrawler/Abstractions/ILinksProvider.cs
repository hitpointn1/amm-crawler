using AMMCrawler.ServiceLayer.DTO;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMMCrawler.Abstractions
{
    public interface ILinksProvider
    {
        Task<HashSet<LinkDataDto>> GetLinksFromPage(IWebDriver driver, string selector);
    }
}
