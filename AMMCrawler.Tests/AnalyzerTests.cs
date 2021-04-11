using AMMCrawler.Core.Enums;
using AMMCrawler.Core.Helpers;
using NUnit.Framework;

namespace AMMCrawler.Tests
{
    public class AnalyzerTests
    {
        [TestCase("http://www.example.usv.ru/", "", ETCType.Undefined)]
        [TestCase("www.example.usv.ru/#", "", ETCType.JavaScript)]
        [TestCase("http://www.example.usv.ru/#", "", ETCType.JavaScript)]
        [TestCase("http://www.example.usv.ru/asd/#tab1", "", ETCType.JavaScript)]
        [TestCase("http://www.example.usv.ru/", "return false;", ETCType.JavaScript)]
        [TestCase("http://www.example.usv.ru/asd.jpg", "", ETCType.Picture)]
        [TestCase("http://www.example.usv.ru/asd1/asd2/asd.jpg", "", ETCType.Picture)]
        [TestCase("http://www.example.usv.ru/asd1/asd2/asd.docx", "", ETCType.Document)]
        [TestCase("http://www.example.usv.ru/login.php", "", ETCType.Script)]
        [TestCase("http://www.example.usv.ru/login.php?username=example", "", ETCType.Script)]
        [TestCase("mailto:example@usv.ru", "", ETCType.Mail)]
        public void LinkData_GetType_IsRight(string url, string onclick, ETCType expectedResult)
        {
            ETCType actualRes = ETCLinksAnalyzer.Instance.GetETCType(url, onclick);
            Assert.AreEqual(expectedResult, actualRes);
        }

        [TestCase("http://www.example.usv.ru/", "http://www.example.usv.ru")]
        [TestCase("https://www.example.usv.ru/", "https://www.example.usv.ru")]
        [TestCase("www.example.usv.ru/", "www.example.usv.ru")]
        [TestCase("http://www.example.usv.ru/abitur/activities/4009/", "http://www.example.usv.ru")]
        public void CrawledLink_GetOrigin_IsRight(string crawledUrl, string originUrlExpected)
        {
            string originUrl = ETCLinksAnalyzer.Instance.GetOrigin(crawledUrl);
            Assert.AreEqual(originUrlExpected, originUrl);
        }

        [TestCase("http://www.example.usv.ru/", "www.example.usv.ru/")]
        [TestCase("https://www.example.usv.ru/", "www.example.usv.ru/")]
        [TestCase("www.example.usv.ru/", "www.example.usv.ru/")]
        public void Link_GetNoProtocolUrl_IsRight(string link, string noProtocolUrlExpected)
        {
            string noProtocolUrl = ETCLinksAnalyzer.Instance.GetNoProtocolUrl(link);
            Assert.AreEqual(noProtocolUrlExpected, noProtocolUrl);
        }

        [TestCase("http://www.example.usv.ru/", ProtocolType.HTTP)]
        [TestCase("https://www.example.usv.ru/", ProtocolType.HTTPS)]
        [TestCase("www.example.usv.ru/", ProtocolType.Unspecified)]
        public void Link_GetProtocolTyp_IsRight(string link, ProtocolType expectedType)
        {
            ProtocolType type = ETCLinksAnalyzer.Instance.GetProtocolType(link);
            Assert.AreEqual(expectedType, type);
        }
    }
}
