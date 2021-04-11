using AMMCrawler.Core.Extensions;
using AMMCrawler.Core.Helpers;
using AMMCrawler.Providers;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMMCrawler.Crawlers
{
    public class InnerLinksDispatcher : BaseLinkDispatcher
    {
        public InnerLinksDispatcher(ILinksService linksService, LinksProvider provider)
            : base(linksService, provider) { }

        protected override string GetQuery(string clearUrl)
        {
            return new LinkSelectorBuilder()
                .HrefStartsWith(clearUrl)
                .NotEtc()
                .OrHrefStartsWith("/")
                .NotEtc()
                .Build();
        }

        protected override Task<int> PerformSave(ResourceLinkDto linkDto, HashSet<LinkDataDto> links)
        {
            return _linksService.SaveInnerLinks(linkDto, links);
        }
    }
}
