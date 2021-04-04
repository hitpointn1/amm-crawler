using AMMCrawler.Abstractions;
using AMMCrawler.Entities;
using AMMCrawler.Enums;
using AMMCrawler.Providers;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMMCrawler.Helpers;
using System.Linq;

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

            string clearUrl = _driver.GetJSResult<string>("return location.origin;");

            string etcLinksQuery = new LinkSelectorBuilder()
                .IsEtc()
                .Build();

            string outerLinksQuery = new LinkSelectorBuilder()
                .HrefStartsWith("http")
                .NotHrefStartsWith(clearUrl)
                .Build();

            string innerLinksQuery = new LinkSelectorBuilder()
                .HrefStartsWith(clearUrl)
                .NotEtc()
                .OrHrefStartsWith("/")
                .NotEtc()
                .Build();

            Task<HashSet<LinkData>> etcLinksTask = _providerFactory.ETCLinksProvider.GetLinksFromPage(_driver, etcLinksQuery);
            Task<HashSet<LinkData>> outerLinksTask = _providerFactory.LinksProvider.GetLinksFromPage(_driver, outerLinksQuery);
            Task<HashSet<LinkData>> innerLinksTask = _providerFactory.LinksProvider.GetLinksFromPage(_driver, innerLinksQuery);

            await Task.WhenAll(etcLinksTask, outerLinksTask, innerLinksTask);

            HashSet<LinkData> etcLinks = etcLinksTask.GetAwaiter().GetResult();
            HashSet<LinkData> outerLinks = outerLinksTask.GetAwaiter().GetResult();
            HashSet<LinkData> innerLinks = innerLinksTask.GetAwaiter().GetResult();
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
