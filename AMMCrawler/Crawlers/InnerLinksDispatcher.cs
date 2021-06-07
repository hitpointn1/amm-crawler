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

        protected override string GetQuery(string httpUrl, string httpsUrl)
        {
            return new LinkSelectorBuilder()
                .Href()
                .NotEtc()
                .Build();
        }

        protected override Task<int> PerformSave(ResourceLinkDto linkDto, HashSet<LinkDataDto> links)
        {
            links.RemoveWhere(l => !l.Href.Contains("://" + _origin));
            return _linksService.SaveInnerLinks(linkDto, links);
        }
    }
}
