namespace AMMCrawler.Abstractions
{
    public interface ICrawlDispatchersFactory
    {
        ICrawlDispatcher InnerLinksCrawler { get; }
        ICrawlDispatcher ETCLinksCrawler { get; }
        ICrawlDispatcher OuterLinksCrawler { get; }
    }
}
