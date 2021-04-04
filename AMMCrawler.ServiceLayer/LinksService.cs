using AMMCrawler.Core.Abstractions;
using AMMCrawler.Core.Enums;
using AMMCrawler.DAL;
using AMMCrawler.DAL.Entities;
using AMMCrawler.DTO;
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

        public async Task<ResourceLinkDto> GetAvailableLink()
        {
            ResourceLink linkData = await _context.ResourceLinks
                .AsNoTracking()
                .FirstOrDefaultAsync(r => !r.IsCrawled && r.Type == LinkType.Inner);
            return new ResourceLinkDto()
            {
                ApplicationID = linkData.ApplicationID,
                URL = linkData.URL
            };
        }

        public async Task SetResourceLinkAsCrawled(ResourceLinkDto dto)
        {
            ResourceLink[] links = await _context.ResourceLinks
               .AsNoTracking()
               .Where(r => !r.IsCrawled
                    && r.Type == LinkType.Inner
                    && r.ApplicationID == dto.ApplicationID)
               .ToArrayAsync();

            _logger.LogInfo("Link to crawl left count - " + (links.Length - 1));

            ResourceLink linkData = links.FirstOrDefault(r => !r.IsCrawled
                    && r.Type == LinkType.Inner
                    && r.URL == dto.URL
                    && r.ApplicationID == dto.ApplicationID);

            if (linkData == null)
                throw new ArgumentException(dto.URL + " Was not found");
            linkData.IsCrawled = true;
            await _context.SaveChangesAsync();
        }

        public Task<int> SaveInnerLinks(ResourceLinkDto crawledLink, HashSet<LinkDataDto> innerLinks)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveOuterLinks(ResourceLinkDto crawledLink, HashSet<LinkDataDto> outerLinks)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveEtcLinks(ResourceLinkDto crawledLink, HashSet<LinkDataDto> etcLinks)
        {
            throw new NotImplementedException();
        }
    }
}
