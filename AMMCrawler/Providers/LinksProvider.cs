using AMMCrawler.Abstractions;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMMCrawler.Providers
{
    internal class LinksProvider : ILinksProvider
    {
        public Task<HashSet<string>> GetLinksFromPage(IWebDriver driver, string selector)
        {
            return Task.Run(() =>
            {
                string script = string.Format("return document.querySelectorAll({0})", selector);
                return driver.GetJSResult<IReadOnlyCollection<IWebElement>>(script);
            })
                .ContinueWith(ParseResult);
        }

        private HashSet<string> ParseResult(Task<IReadOnlyCollection<IWebElement>> elementsTask)
        {
            IReadOnlyCollection<IWebElement> elements = elementsTask.GetAwaiter().GetResult();

            return elements.Select(GetLink)
                .Where(l => !string.IsNullOrEmpty(l))
                .ToHashSet();
        }

        private string GetLink(IWebElement element)
        {
            string href = element.GetAttribute("href");

            if (!IsLinkValid(href))
                return null;

            return ParseLink(href);
        }

        protected virtual bool IsLinkValid(string href)
        {
            return !string.IsNullOrEmpty(href) && !href.StartsWith("mailto:");
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
