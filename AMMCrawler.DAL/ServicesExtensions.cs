using AMMCrawler.Core;
using AMMCrawler.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AMMCrawler.Configuration
{
    public static partial class ServicesExtensions
    {
        public static IServiceCollection ConfigureDataSource(this IServiceCollection services)
        {
            return services
                .AddDbContext<CrawlerContext>(o => o.UseSqlite(CrawlConfiguration.CrawlerContext), ServiceLifetime.Transient);
        }
    }
}
