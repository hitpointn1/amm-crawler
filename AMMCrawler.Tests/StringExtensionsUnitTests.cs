using AMMCrawler.Core.Helpers;
using NUnit.Framework;

namespace AMMCrawler.Tests
{
    public class StringExtensionsUnitTests
    {

        [TestCase(null, "anything", null)]
        [TestCase("", "anything", "")]
        [TestCase("http://example.com/", "http://example.com/", "http://example.com/")]
        [TestCase("http://example.com/", "http://exampel.com/", "http://example.com/")]
        [TestCase("http://example.com/news", "http://example.com/", "news")]
        public void RemoveSubstring_DifferentCases_ExpectedResult(string main, string substr, string output)
        {
            string result = main.Remove(substr);

            Assert.That(result, Is.EqualTo(output));
        }
    }
}
