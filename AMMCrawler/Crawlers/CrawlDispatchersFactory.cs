using AMMCrawler.Abstractions;

namespace AMMCrawler.Crawlers
{
    public class CrawlDispatchersFactory : ICrawlDispatchersFactory
    {
        public CrawlDispatchersFactory(InnerLinksDispatcher innerLinksDispatcher, OuterLinksDispatcher outerLinksDispatcher, ETCLinksDispatcher etcLinksDispatcher)
        {
            InnerLinksCrawler = innerLinksDispatcher;
            ETCLinksCrawler = etcLinksDispatcher;
            OuterLinksCrawler = outerLinksDispatcher;
        }

        public ICrawlDispatcher InnerLinksCrawler { get; }

        public ICrawlDispatcher ETCLinksCrawler { get; }

        public ICrawlDispatcher OuterLinksCrawler { get; }
    }
}
