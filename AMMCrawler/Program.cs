using AMMCrawler.Abstractions;
using System.Threading.Tasks;

namespace AMMCrawler
{
    internal class Program
    {
        private static readonly IConsoleApplication Crawler = new CrawlerApplication();
        internal static async Task Main(string[] args)
        {
            await Crawler.Run();
        }
    }
}
