using OpenQA.Selenium;

namespace AMMCrawler.Providers
{
    internal class ETCLinksProvider : LinksProvider
    {
        protected override string ParseLink(string href)
        {
            int indexOfQM = href.IndexOf('?');
            if (indexOfQM != -1)
                return href.Substring(0, indexOfQM);
            return href;
        }
    }
}
