using AMMCrawler.Core.Abstractions;
using AMMCrawler.Core.Enums;
using AMMCrawler.DAL;
using AMMCrawler.DAL.Entities;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMMCrawler.ServiceLayer
{
    public class LinksService : ILinksService
    {
        private readonly CrawlerContext _context;
        private readonly ILogger _logger;

        public LinksService(CrawlerContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveCrawledLinkIfMissing(ResourceLinkDto crawledLinkDto)
        {
            ResourceLink crawledLink = await _context.ResourceLinks
                 .AsNoTracking()
                 .FirstOrDefaultAsync(r => r.URL == crawledLinkDto.URL && r.Type == LinkType.Inner);
            if (crawledLink is not null)
                return;
            crawledLink = new ResourceLink()
            {
                ApplicationID = crawledLinkDto.ApplicationID,
                IsCrawled = false,
                Type = LinkType.Inner,
                URL = crawledLinkDto.URL
            };
            await _context.AddAsync(crawledLink);
            await _context.SaveChangesAsync();
        }

        public async Task<ResourceLinkDto> GetAvailableLink(int applicationID)
        {
            ResourceLink linkData = await _context.ResourceLinks
                .AsNoTracking()
                .FirstOrDefaultAsync(r => !r.IsCrawled && r.Type == LinkType.Inner && r.ApplicationID == applicationID);
            if (linkData is null)
                return null;
            return new ResourceLinkDto()
            {
                ApplicationID = linkData.ApplicationID,
                URL = linkData.URL
            };
        }

        public async Task SetResourceLinkAsCrawled(ResourceLinkDto dto)
        {
            ResourceLink[] links = await _context.ResourceLinks
               .Where(r => !r.IsCrawled
                    && r.Type == LinkType.Inner
                    && r.ApplicationID == dto.ApplicationID)
               .ToArrayAsync();

            _logger.LogInfo("Link to crawl left count - " + links.Length);
            if (links.Length == 0)
                return;

            ResourceLink linkData = links.FirstOrDefault(r => !r.IsCrawled
                    && r.Type == LinkType.Inner
                    && r.URL == dto.URL
                    && r.ApplicationID == dto.ApplicationID);

            if (linkData is null)
                throw new ArgumentException(dto.URL + " Was not found");
            linkData.IsCrawled = true;
            await _context.SaveChangesAsync();
        }

        public Task<int> SaveInnerLinks(ResourceLinkDto crawledLinkDto, HashSet<LinkDataDto> innerLinks)
        {
            return SaveLinksCount(crawledLinkDto, innerLinks, LinkType.Inner);
        }

        public Task<int> SaveOuterLinks(ResourceLinkDto crawledLinkDto, HashSet<LinkDataDto> outerLinks)
        {
            return SaveLinksCount(crawledLinkDto, outerLinks, LinkType.Outer);
        }

        public async Task<int> SaveEtcLinks(ResourceLinkDto crawledLink, HashSet<LinkDataDto> etcLinks)
        {
            IEnumerable<ResourceLink> links = await SaveLinks(crawledLink, etcLinks, LinkType.Etc);
            IEnumerable<ETCLinkMetadata> etcLinksMetadata = links.Join(etcLinks,
                (rl) => rl.URL,
                (rldto) => rldto.Href,
                (rl, rldto) => new EtcLinkDto
                {
                    ResourceLinkID = rl.ID,
                    Href = rldto.Href,
                    OnClick = rldto.OnClick
                })
                .Select(etc => etc.GetETCLinkMetadata(crawledLink));
            await _context.AddRangeAsync(etcLinksMetadata);
            await _context.SaveChangesAsync();
            return links.Count();
        }

        private Task<int> SaveLinksCount(ResourceLinkDto crawledLinkDto, HashSet<LinkDataDto> linksDtos, LinkType type)
        {
            return SaveLinks(crawledLinkDto, linksDtos, type).ContinueWith(res => res.GetAwaiter().GetResult().Count());
        }

        private async Task<IEnumerable<ResourceLink>> SaveLinks(ResourceLinkDto crawledLinkDto, HashSet<LinkDataDto> linksDtos, LinkType type)
        {
            Task<ResourceLink> crawledLinkTask = _context.ResourceLinks
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.URL == crawledLinkDto.URL && r.Type == LinkType.Inner);

            IEnumerable<string> links = linksDtos
                .Select(s => s.Href);

            Task<ResourceLink[]> existedLinksTask = _context.ResourceLinks
                .AsNoTracking()
                .Where(r => links.Contains(r.URL) && r.Type == type)
                .ToArrayAsync();

            ResourceLink crawledLink = await crawledLinkTask;
            ResourceLink[] existedLinks = await existedLinksTask;
            ResourceLink[] notAddedLinks = links
                .Except(existedLinks.Select(r => r.URL))
                .Where(u => u != crawledLink.URL)
                .Select(r => new ResourceLink()
                {
                    ApplicationID = crawledLink.ApplicationID,
                    IsCrawled = false,
                    Type = type,
                    URL = r
                })
                .ToArray();
            await _context.AddRangeAsync(notAddedLinks);
            await _context.SaveChangesAsync();
            if (type != LinkType.Inner)
                return notAddedLinks;
            IEnumerable<ResourceCrawlMapping> resourceCrawls = existedLinks
                .Concat(notAddedLinks)
                .Select(l => new ResourceCrawlMapping()
                {
                    CrawledLinkID = crawledLink.ID,
                    FoundLinkID = l.ID
                });

            IEnumerable<int> crawledLinkIds = resourceCrawls.Select(r => r.CrawledLinkID).Distinct();
            IEnumerable<int> foundLinkIds = resourceCrawls.Select(r => r.FoundLinkID);

            ResourceCrawlMapping[] existedMappings = await _context.ResourceMappings
                .Where(r => crawledLinkIds.Contains(r.CrawledLinkID))
                .Where(r => foundLinkIds.Contains(r.FoundLinkID))
                .ToArrayAsync();

            IEnumerable<ResourceCrawlMapping> mappingsToAdd = resourceCrawls
                .Where(r => !resourceCrawls.Any(c => c.CrawledLinkID == r.CrawledLinkID && c.FoundLinkID == r.FoundLinkID));
            await _context.AddRangeAsync(mappingsToAdd);
            await _context.SaveChangesAsync();
            return notAddedLinks;
        }
    }
}
