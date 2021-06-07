using AMMCrawler.Abstractions;
using OpenQA.Selenium;
using System.Threading.Tasks;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using AMMCrawler.Core.Abstractions;
using AMMCrawler.Core.Helpers;
using System;
using AMMCrawler.Extensions;
using System.Collections.Generic;

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
            string startMessage = string.Format("Crawl for {0} is started", dto.URL);
            _logger.LogInfo(startMessage);

            string driverOrigin = _driver.GetJSResult<string>("return location.origin;");
            string clearUrl = ETCLinksAnalyzer.Instance.GetOrigin(dto.URL);
            await _linksService.SaveCrawledLinkIfMissing(dto);

            if (driverOrigin != clearUrl)
            {
                await HandleRedirect(dto, clearUrl, driverOrigin);
                return;
            }

            int savedInnerLinksCount = await _crawlFactory.InnerLinksCrawler.PerformCrawl(_driver, dto, clearUrl);

            await _linksService.SetResourceLinkAsCrawled(dto);

            string endMessage = string.Format("{0} is crawled succesfully. New inner links count - {1}",
                dto.URL, savedInnerLinksCount);
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

        private async Task HandleRedirect(ResourceLinkDto dto, string clearUrl, string driverOrigin)
        {
            string message = string.Format("Redirected from {0} to {1}", clearUrl, driverOrigin);
            _logger.LogError(message);
            string noProtoDriver = ETCLinksAnalyzer.Instance.GetNoProtocolUrl(driverOrigin);
            string noProtoUrl = ETCLinksAnalyzer.Instance.GetNoProtocolUrl(clearUrl);
            if (noProtoDriver == noProtoUrl)
            {
                var newLink = new LinkDataDto()
                {
                    Href = _driver.Url
                };
                await _linksService.SaveInnerLinks(dto, new HashSet<LinkDataDto>(new LinkDataDto[] { newLink }));
            }
            await _linksService.SetResourceLinkAsCrawled(dto);
        }
    }
}
