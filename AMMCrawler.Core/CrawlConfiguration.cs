using Microsoft.Extensions.Configuration;
using System.IO;

namespace AMMCrawler.Core
{
    public static class CrawlConfiguration
    {

        static CrawlConfiguration()
        {
            IConfiguration configuration = GetConfiguration();
            Application = configuration.GetValue<string>(nameof(Application));
            InitialLink = configuration.GetValue<string>(nameof(InitialLink));
            CrawlerContext = configuration.GetConnectionString(nameof(CrawlerContext));
        }

        public static string Application { get; }
        public static string InitialLink { get; }
        public static string CrawlerContext { get; }

        private static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            return configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
