using AMMCrawler.Abstractions;
using AMMCrawler.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AMMCrawler
{
    internal class AMMCrawler : ICrawler
    {
        private readonly CrawlerContext _context;
        public AMMCrawler(CrawlerContext context)
        {
            _context = context;
        }

        public Task Crawl(string url)
        {
            return Task.Run(() => { });
        }
        
        public async Task<ResourceLink> GetAvailableLink()
        {
            return await _context.ResourceLinks.FirstOrDefaultAsync(r => !r.IsCrawled);
        }
    }
}
