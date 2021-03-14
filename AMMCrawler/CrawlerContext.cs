using AMMCrawler.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMMCrawler
{
    public class CrawlerContext : DbContext
    {

        public DbSet<ResourceLink> ResourceLinks { get; set; }
        public DbSet<ResourceCrawlMapping> ResourceMappings { get; set; }
    }
}
