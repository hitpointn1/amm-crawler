using AMMCrawler.Core.Enums;
using System;
using System.Linq;

namespace AMMCrawler.Core.Helpers
{
    public class ETCLinksAnalyzer
    {
        private const string HTTP_PROTOCOL = "http://";
        private const string HTTPS_PROTOCOL = "https://";
        private static string[] PictureIdentifier =
        {
            ".png",
            ".jpg",
            ".jpeg",
            ".gif"
        };
        private static string[] DocIdentifier =
        {
            ".doc",
            ".docx",
            ".xls",
            ".xlsx",
            ".pdf",
            ".csv",
            ".pptx",
            ".txt"
        };

        public static ETCLinksAnalyzer Instance { get; } = new ETCLinksAnalyzer();
        private ETCLinksAnalyzer() { }

        public ETCType GetETCType(string url, string onclick)
        {
            if (url.StartsWith("mailto:"))
                return ETCType.Mail;
            if (url.EndsWith(".php") || url.Contains(".php?"))
                return ETCType.Script;
            int urlEndIndex = url.LastIndexOf('/');
            string urlEnd = url;
            if (urlEndIndex > -1)
                urlEnd = urlEnd.Substring(urlEndIndex);
            if (PictureIdentifier.Any(p => urlEnd.EndsWith(p)))
                return ETCType.Picture;
            if (DocIdentifier.Any(p => urlEnd.EndsWith(p)))
                return ETCType.Document;
            if (urlEnd.Contains('#') || !string.IsNullOrEmpty(onclick))
                return ETCType.JavaScript;
            return ETCType.Undefined;
        }

        public string GetOrigin(string url)
        {
            ProtocolType type = GetProtocolType(url);
            int protocolIndex = url.IndexOf('/', Convert.ToInt32(type));
            if (protocolIndex > -1)
                return url.Substring(0, protocolIndex);

            return url;
        }

        public ProtocolType GetProtocolType(string url)
        {
            if (url.StartsWith(HTTPS_PROTOCOL))
                return ProtocolType.HTTPS;

            if (url.StartsWith(HTTP_PROTOCOL))
                return ProtocolType.HTTP;

            return ProtocolType.Unspecified;
        }

        public string GetNoProtocolUrl(string url)
        {
            int protocolIndex = url.IndexOf(HTTPS_PROTOCOL);
            if (protocolIndex > -1)
                return url.Substring(protocolIndex + HTTPS_PROTOCOL.Length);

            protocolIndex = url.IndexOf(HTTP_PROTOCOL);
            if (protocolIndex > -1)
                return url.Substring(protocolIndex + HTTP_PROTOCOL.Length);

            return url;
        }
    }
}
