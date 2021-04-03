using AMMCrawler.Abstractions;

namespace AMMCrawler.Providers
{
    internal class LinksProviderFactory : ILinksProviderFactory
    {
        public LinksProviderFactory(LinksProvider linksProvider, ETCLinksProvider etcLinksProvider)
        {
            LinksProvider = linksProvider;
            ETCLinksProvider = etcLinksProvider;
        }

        public ILinksProvider LinksProvider { get; }

        public ILinksProvider ETCLinksProvider { get; }
    }
}
