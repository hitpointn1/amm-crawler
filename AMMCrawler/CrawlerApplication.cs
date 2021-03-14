using AMMCrawler.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace AMMCrawler
{
    internal class CrawlerApplication : IConsoleApplication
    {
        public CrawlerApplication()
        {
            Configuration = GetConfiguration();
        }

        public IConfiguration Configuration { get; }

        public async Task Run()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<ICrawler, AMMCrawler>();
                    services.AddDbContext<CrawlerContext>(o => o.UseSqlite(Configuration.GetConnectionString(nameof(CrawlerContext))), ServiceLifetime.Transient);
                })
                .Build();

            var crawler = host.Services.GetRequiredService<ICrawler>();

            await crawler.Crawl();
        }

        internal static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            return configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
