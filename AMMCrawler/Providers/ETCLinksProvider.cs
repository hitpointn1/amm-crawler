using OpenQA.Selenium;

namespace AMMCrawler.Providers
{
    internal class ETCLinksProvider : LinksProvider
    {
        protected override string ParseLink(string href)
        {
            return href;
        }
    }
}
