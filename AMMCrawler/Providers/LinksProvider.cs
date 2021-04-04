﻿using AMMCrawler.Abstractions;
using AMMCrawler.Extensions;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMMCrawler.Providers
{
    internal class LinksProvider : ILinksProvider
    {
        public Task<HashSet<LinkData>> GetLinksFromPage(IWebDriver driver, string selector)
        {
            return Task.Run(() =>
            {
                string script = string.Format("return document.querySelectorAll({0})", selector);
                return driver.GetJSResult<IReadOnlyCollection<IWebElement>>(script);
            })
                .ContinueWith(ParseResult);
        }

        private HashSet<LinkData> ParseResult(Task<IReadOnlyCollection<IWebElement>> elementsTask)
        {
            IReadOnlyCollection<IWebElement> elements = elementsTask.GetAwaiter().GetResult();
            return elements.Select(GetLink)
                .Where(l => l != null)
                .ToHashSet();
        }

        private LinkData GetLink(IWebElement element)
        {
            string href = element.GetAttribute("href");
            string onclick = element.GetAttribute("onclick");

            if (!IsLinkValid(href))
                return null;

            return new LinkData()
            {
                Href = ParseLink(href),
                OnClick = onclick
            };
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
