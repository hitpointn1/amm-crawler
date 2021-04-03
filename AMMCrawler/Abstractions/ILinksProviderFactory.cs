namespace AMMCrawler.Abstractions
{
    interface ILinksProviderFactory
    {
        ILinksProvider LinksProvider { get; }
        ILinksProvider ETCLinksProvider { get; }
    }
}
