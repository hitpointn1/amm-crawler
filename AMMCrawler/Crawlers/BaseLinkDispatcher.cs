using AMMCrawler.Abstractions;
using AMMCrawler.Core.Helpers;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMMCrawler.Crawlers
{
    public abstract class BaseLinkDispatcher : ICrawlDispatcher
    {
        protected readonly ILinksService _linksService;
        private ILinksProvider _linksProvider;

        protected BaseLinkDispatcher(ILinksService linksService, ILinksProvider provider)
        {
            _linksService = linksService;
            _linksProvider = provider;
        }

        public async Task<int> PerformCrawl(IWebDriver driver, ResourceLinkDto linkDto, string clearUrl)
        {
            string origin = ETCLinksAnalyzer.Instance.GetNoProtocolUrl(clearUrl);
            string httpsUrl = ETCLinksAnalyzer.HTTPS_PROTOCOL + origin;
            string httpUrl = ETCLinksAnalyzer.HTTP_PROTOCOL + origin;
            string query = GetQuery(httpUrl, httpsUrl);
            Task<int> innerLinksTask = await _linksProvider
               .GetLinksFromPage(driver, query)
               .ContinueWith((r) => PerformSave(linkDto, r.GetAwaiter().GetResult()));
            return await innerLinksTask;
        }

        protected abstract string GetQuery(string httpUrl, string httpsUrl);
        protected abstract Task<int> PerformSave(ResourceLinkDto linkDto, HashSet<LinkDataDto> links);
    }
}
