using AMMCrawler.DTO;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using AMMCrawler.Core.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMMCrawler.Providers;
using AMMCrawler.Core.Helpers;

namespace AMMCrawler.Crawlers
{
    public class ETCLinksDispatcher : BaseLinkDispatcher
    {
        public ETCLinksDispatcher(ILinksService linksService, ETCLinksProvider provider)
            : base(linksService, provider) { }

        protected override string GetQuery(string clearUrl)
        {
            return new LinkSelectorBuilder()
                .IsEtc()
                .Build();
        }

        protected override Task<int> PerformSave(ResourceLinkDto linkDto, HashSet<LinkDataDto> links)
        {
            return _linksService.SaveOuterLinks(linkDto, links);
        }
    }
}
