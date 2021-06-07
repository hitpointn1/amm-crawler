using AMMCrawler.Abstractions;
using AMMCrawler.Extensions;
using AMMCrawler.ServiceLayer.DTO;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AMMCrawler.Providers
{
    public class LinksProvider : ILinksProvider
    {
        private const int MAX_RETRY_COUNT = 3;
        public Task<HashSet<LinkDataDto>> GetLinksFromPage(IWebDriver driver, string selector)
        {
            return Task.Run(() =>
            {
                string script = string.Format("return document.querySelectorAll({0})", selector);
                return driver.GetJSResult<IReadOnlyCollection<IWebElement>>(script);
            })
                .ContinueWith(ParseResult);
        }

        private HashSet<LinkDataDto> ParseResult(Task<IReadOnlyCollection<IWebElement>> elementsTask)
        {
            IReadOnlyCollection<IWebElement> elements = elementsTask.GetAwaiter().GetResult();
            if (elements == null)
                return new HashSet<LinkDataDto>(0);
            return elements.Select(GetLink)
                .Where(l => l != null)
                .ToHashSet();
        }

        private LinkDataDto GetLink(IWebElement element, int retryNumber = 0)
        {
            try
            {
                string href = element.GetAttribute("href");
                string decodedUrl = HttpUtility.UrlDecode(href);
                string onclick = element.GetAttribute("onclick");

                if (!IsLinkValid(decodedUrl))
                    return null;

                return new LinkDataDto()
                {
                    Href = ParseLink(decodedUrl),
                    OnClick = onclick
                };
            }
            catch
            {
                if (retryNumber == MAX_RETRY_COUNT)
                    return null;
                Task.Delay(1000).GetAwaiter().GetResult();
                return GetLink(element, retryNumber + 1);
            }
           
        }

        protected virtual bool IsLinkValid(string href)
        {
            return !string.IsNullOrEmpty(href);
        }

        protected virtual string ParseLink(string href)
        {
            int indexOfQM = href.IndexOf('?');
            if (indexOfQM != -1)
                return href.Substring(0, indexOfQM);

            int indexOfH = href.IndexOf('#');
            if (indexOfH != -1)
                return href.Substring(0, indexOfH);

            return href;
        }
    }
}
