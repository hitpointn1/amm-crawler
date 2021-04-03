using AMMCrawler.Abstractions;
using AMMCrawler.Entities;
using AMMCrawler.Enums;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMMCrawler
{
    internal class AMMCrawler : ICrawler
    {
        private readonly CrawlerContext _context;
        private readonly IWebDriver _driver;
        private readonly ILinksProviderFactory _providerFactory;
        private const int JSWait = 500;
        public AMMCrawler(CrawlerContext context, IWebDriver driver, ILinksProviderFactory providerFactory)
        {
            _context = context;
            _driver = driver;
            _providerFactory = providerFactory;
        }

        public async Task Crawl(string url)
        {
            _driver.Navigate().GoToUrl(url);
            await Task.Delay(JSWait);

            string clearUrl = _driver.GetJSResult<string>("return location.origin + location.pathname;");

            string etcLinksQuery = "'a[href^=\"#\"], a[href^=\"/\"][onclick^=\"return false\"]'";
            string outerLinksQuery = "'a[href^=\"http\"]:not([href*=\"" + clearUrl + "\"])'";
            string innerLinksQuery = "'a[href=\"" + clearUrl + "\"], a[href^=\"/\"]:not([href$=\".*\"]):not([onclick^=\"return false\"])'";

            Task<HashSet<string>> etcLinksTask = _providerFactory.ETCLinksProvider.GetLinksFromPage(_driver, etcLinksQuery);
            Task<HashSet<string>> outerLinksTask = _providerFactory.LinksProvider.GetLinksFromPage(_driver, outerLinksQuery);
            Task<HashSet<string>> innerLinksTask = _providerFactory.LinksProvider.GetLinksFromPage(_driver, innerLinksQuery);

            await Task.WhenAll(etcLinksTask, outerLinksTask, innerLinksTask);

            HashSet<string> etcLinks = etcLinksTask.GetAwaiter().GetResult();
            HashSet<string> outerLinks = outerLinksTask.GetAwaiter().GetResult();
            HashSet<string> innerLinks = innerLinksTask.GetAwaiter().GetResult();
        }
        
        public Task<ResourceLink> GetAvailableLink()
        {
            return _context.ResourceLinks.FirstOrDefaultAsync(r => !r.IsCrawled && r.Type != LinkType.Etc);
        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }
    }
}
