
using OpenQA.Selenium.Chrome;

namespace AMMCrawler.Providers
{
    public class ChromeDriverProvider
    {
        public ChromeDriver GetChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            return new ChromeDriver(options);
        }
    }
}
