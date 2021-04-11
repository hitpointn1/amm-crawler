using AMMCrawler.Abstractions;
using OpenQA.Selenium;
using System.Threading.Tasks;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using AMMCrawler.Core.Abstractions;
using AMMCrawler.Core.Helpers;

namespace AMMCrawler
{
    internal class AMMCrawler : ICrawler
    {
        private readonly IWebDriver _driver;
        private readonly ILinksService _linksService;
        private readonly ILogger _logger;
        private readonly ICrawlDispatchersFactory _crawlFactory;

        public AMMCrawler(ILinksService linksService, IWebDriver driver, ILogger logger, ICrawlDispatchersFactory factory)
        {
            _driver = driver;
            _linksService = linksService;
            _logger = logger;
            _crawlFactory = factory;
        }

        public async Task Crawl(ResourceLinkDto dto)
        {
            _driver.Navigate().GoToUrl(dto.URL);
            Task saveTask = _linksService.SaveCrawledLinkIfMissing(dto);
            string startMessage = string.Format("Crawl for {0} is started", dto.URL);
            _logger.LogInfo(startMessage);

            string clearUrl = ETCLinksAnalyzer.Instance.GetOrigin(dto.URL);

            await saveTask;
            Task<int> innerLinksTask = _crawlFactory.InnerLinksCrawler.PerformCrawl(_driver, dto, clearUrl);
            Task<int> etcLinksTask = _crawlFactory.ETCLinksCrawler.PerformCrawl(_driver, dto, clearUrl);
            Task<int> outerLinksTask = _crawlFactory.OuterLinksCrawler.PerformCrawl(_driver, dto, clearUrl);

            await Task.WhenAll(etcLinksTask, outerLinksTask, innerLinksTask);

            int savedInnerLinksCount = innerLinksTask.GetAwaiter().GetResult();
            int savedEtcLinksCount = etcLinksTask.GetAwaiter().GetResult();
            int savedOuterLinksCount = outerLinksTask.GetAwaiter().GetResult();

            await _linksService.SetResourceLinkAsCrawled(dto);

            string endMessage = string.Format("{0} is crawled succesfully. New inner links count - {1}, New outer links count - {2}, New etc links - {3}"
                , dto.URL, savedInnerLinksCount, savedOuterLinksCount, savedEtcLinksCount);
            _logger.LogInfo(endMessage);
        }

        public Task<ResourceLinkDto> GetAvailableLink(int applicationID)
        {
            return _linksService.GetAvailableLink(applicationID);
        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }
    }
}
