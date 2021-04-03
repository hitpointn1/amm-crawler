using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AMMCrawler
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static T GetJSResult<T>(this IWebDriver driver, string jsCommand)
            where T : class
        {
            return ((IJavaScriptExecutor)driver).ExecuteScript(jsCommand) as T;
        }

        public static IWebElement FindElementByJs(this IWebDriver driver, string jsCommand)
        {
            return (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(jsCommand);
        }

        public static IWebElement FindElementByJsWithWait(this IWebDriver driver, string jsCommand, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(d => d.FindElementByJs(jsCommand));
            }
            return driver.FindElementByJs(jsCommand);
        }
    }
}
