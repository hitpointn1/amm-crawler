﻿using AMMCrawler.Core.Helpers;
using AMMCrawler.DTO;
using AMMCrawler.Providers;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMMCrawler.Crawlers
{
    public class OuterLinksDispatcher : BaseLinkDispatcher
    {
        public OuterLinksDispatcher(ILinksService linksService, LinksProvider provider)
            : base(linksService, provider) { }

        protected override string GetQuery(string clearUrl)
        {
            return new LinkSelectorBuilder()
                .HrefStartsWith("http")
                .NotHrefStartsWith(clearUrl)
                .Build();
        }

        protected override Task<int> PerformSave(ResourceLinkDto linkDto, HashSet<LinkDataDto> links)
        {
            return _linksService.SaveOuterLinks(linkDto, links);
        }
    }
}
