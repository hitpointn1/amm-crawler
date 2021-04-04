using AMMCrawler.Core.Extensions;
using AMMCrawler.Core.Helpers;
using NUnit.Framework;

namespace AMMCrawler.Tests
{
    public class LinkBuilderUnitTests
    {
        private LinkSelectorBuilder _testBuilder;

        [SetUp]
        public void SetUp()
        {
            _testBuilder = new LinkSelectorBuilder();
        }

        [Test]
        public void TestSelectorBuilder_WithoutConf_HasOnlyATag()
        {
            string linkSelector = _testBuilder.Build();
            Assert.AreEqual("'a'", linkSelector);
        }

        [Test]
        public void TestSelectorBuilder_Emptied_HasOnlyATag()
        {
            var builder = _testBuilder.HrefAny("test");
            builder.Clear();
            string linkSelector = builder.Build();
            Assert.AreEqual("'a'", linkSelector);
        }

        [Test]
        public void TestSelectorBuilder_EtcSelector_MatchesFully()
        {
            string etcLinksQueryExpected = "'a[href^=\"#\"],a[href^=\"mailto:\"]" +
                ",a[href$=\".doc\"]" +
                ",a[href$=\".docx\"]" +
                ",a[href$=\".png\"]" +
                ",a[href$=\".jpg\"]" +
                ",a[href$=\".xls\"]" +
                ",a[href$=\".xlsx\"]" +
                ",a[href$=\".pdf\"]" +
                ",a[href$=\".php\"]" +
                ",a[href*=\".php?\"]" +
                ",a[href^=\"/\"][onclick^=\"return false\"]'";

            string etcLinksQuery = _testBuilder
                .IsEtc()
                .Build();

            Assert.AreEqual(etcLinksQuery, etcLinksQueryExpected);
        }

        [Test]
        public void TestSelectorBuilder_OuterLinksSelector_MatchesFully()
        {
            string url = "http://example.com";
            string outerLinksQueryExpected = "'a[href^=\"http\"]:not([href^=\"" + url + "\"])'";

            string outerLinksQuery = _testBuilder
                .HrefStartsWith("http")
                .NotHrefStartsWith(url)
                .Build();

            Assert.AreEqual(outerLinksQuery, outerLinksQueryExpected);
        }

        [Test]
        public void TestSelectorBuilder_InnnerLinksSelector_MatchesFully()
        {
            string url = "http://example.com";

            string innerLinksQueryExpected = "'a[href^=\"" + url + "\"]" +
                ":not([href$=\".doc\"])" +
                ":not([href$=\".docx\"])" +
                ":not([href$=\".png\"])" +
                ":not([href$=\".jpg\"])" +
                ":not([href$=\".xls\"])" +
                ":not([href$=\".xlsx\"])" +
                ":not([href$=\".pdf\"])" +
                ":not([href^=\"mailto:\"])" +
                ":not([href*=\".php?\"])" +
                ":not([href$=\".php\"])" +
                ":not([onclick^=\"return false\"])" +
                ",a[href^=\"/\"]" +
                ":not([href$=\".doc\"])" +
                ":not([href$=\".docx\"])" +
                ":not([href$=\".png\"])" +
                ":not([href$=\".jpg\"])" +
                ":not([href$=\".xls\"])" +
                ":not([href$=\".xlsx\"])" +
                ":not([href$=\".pdf\"])" +
                ":not([href^=\"mailto:\"])" +
                ":not([href*=\".php?\"])" +
                ":not([href$=\".php\"])" +
                ":not([onclick^=\"return false\"])'";

            string innerLinksQuery = _testBuilder
                .HrefStartsWith(url)
                .NotEtc()
                .OrHrefStartsWith("/")
                .NotEtc()
                .Build();

            Assert.AreEqual(innerLinksQuery, innerLinksQueryExpected);
        }
    }
}
