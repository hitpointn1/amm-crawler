using AMMCrawler.Abstractions;
using AMMCrawler.Entities;
using AMMCrawler.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Threading.Tasks;

namespace AMMCrawler
{
    internal class CrawlerApplication : IConsoleApplication
    {
        static CrawlerApplication()
        {
            Configuration = GetConfiguration();
        }

        public static IConfiguration Configuration { get; }

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

                    services.AddDbContext<CrawlerContext>(o => o.UseSqlite(Configuration.GetConnectionString(nameof(CrawlerContext))), ServiceLifetime.Transient);
                })
                .Build();

            using (ICrawler crawler = host.Services.GetRequiredService<ICrawler>())
                await crawler.Crawl(Configuration.GetValue<string>("InitialLink"));
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

        internal static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            return configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
