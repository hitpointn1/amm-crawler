using AMMCrawler.Abstractions;
using AMMCrawler.Core;
using AMMCrawler.DAL;
using AMMCrawler.DAL.Entities;
using AMMCrawler.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;

namespace AMMCrawler
{
    internal class CrawlerApplication : IConsoleApplication
    {

        public async Task Run()
        {
            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<ICrawler, AMMCrawler>();
                    services.AddTransient<IWebDriver, ChromeDriver>();

                    services.AddSingleton<LinksProvider>();
                    services.AddSingleton<ETCLinksProvider>();
                    services.AddSingleton<ILinksProviderFactory, LinksProviderFactory>();

                    services.AddDbContext<CrawlerContext>(o => o.UseSqlite(CrawlConfiguration.CrawlerContext), ServiceLifetime.Transient);
                })
                .Build();

            using (ICrawler crawler = host.Services.GetRequiredService<ICrawler>())
                await crawler.Crawl(CrawlConfiguration.InitialLink);
            ResourceLink resLink = null;
            do
            {
                using ICrawler currentCrawler = host.Services.GetRequiredService<ICrawler>();
                resLink = await currentCrawler.GetAvailableLink();
                if (resLink is not null)
                    await currentCrawler.Crawl(resLink.URL);

            }
            while (resLink is not null);
        }
    }
}
