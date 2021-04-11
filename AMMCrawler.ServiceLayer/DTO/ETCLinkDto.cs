using AMMCrawler.Core.Helpers;
using AMMCrawler.DAL.Entities;

namespace AMMCrawler.ServiceLayer.DTO
{
    public class EtcLinkDto : LinkDataDto
    {
        public int ResourceLinkID { get; set; }

        public ETCLinkMetadata GetETCLinkMetadata(ResourceLinkDto crawledLink)
        {
            string originalUrl = ETCLinksAnalyzer.Instance.GetOrigin(crawledLink.URL);
            var result = new ETCLinkMetadata()
            {
                ResourceLinkID = ResourceLinkID,
                IsInternal = Href.StartsWith(originalUrl),
                Type = ETCLinksAnalyzer.Instance.GetETCType(Href, OnClick)
            };

            return result;
        }
    }
}
