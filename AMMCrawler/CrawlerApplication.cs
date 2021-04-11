using AMMCrawler.Abstractions;
using AMMCrawler.Configuration;
using AMMCrawler.Core;
using AMMCrawler.Core.Abstractions;
using AMMCrawler.Crawlers;
using AMMCrawler.Providers;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
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
                    services.AddTransient<ChromeDriverProvider>();
                    services.AddTransient<ICrawler, AMMCrawler>();
                    services.AddTransient<IWebDriver, ChromeDriver>(s => s.GetRequiredService<ChromeDriverProvider>().GetChromeDriver());

                    services.AddSingleton<LinksProvider>();
                    services.AddSingleton<ETCLinksProvider>();
                    services.AddSingleton<ILogger, ConsoleLogger>();

                    services.AddSingleton<InnerLinksDispatcher>();
                    services.AddSingleton<ETCLinksDispatcher>();
                    services.AddSingleton<OuterLinksDispatcher>();
                    services.AddSingleton<ICrawlDispatchersFactory, CrawlDispatchersFactory>();

                    services.ConfigureServices();
                })
                .Build();

            string applicationName = CrawlConfiguration.Application;
            string applicationUrl = CrawlConfiguration.InitialLink;
            ApplicationRunDto runInfo = await host.Services
                .GetRequiredService<IApplicationService>()
                .StartCrawl(applicationName, applicationUrl);

            try
            {
                using ICrawler crawler = host.Services.GetRequiredService<ICrawler>();
                ResourceLinkDto resLink = await crawler.GetAvailableLink(runInfo.ApplicationID);
                if (resLink is null)
                    resLink = new ResourceLinkDto()
                    {
                        ApplicationID = runInfo.ApplicationID,
                        URL = applicationUrl
                    };

                await crawler.Crawl(resLink);
                do
                {
                    using ICrawler currentCrawler = host.Services.GetRequiredService<ICrawler>();
                    resLink = await currentCrawler.GetAvailableLink(runInfo.ApplicationID);
                    if (resLink is not null)
                        await currentCrawler.Crawl(resLink);

                }
                while (resLink is not null);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                host.Services.GetRequiredService<ILogger>().LogError("Error in crawl", ex);
            }
            finally
            {
                await host.Services
                    .GetRequiredService<IApplicationService>()
                    .EndCrawl(runInfo.RunID);
            }         
        }
    }
}
