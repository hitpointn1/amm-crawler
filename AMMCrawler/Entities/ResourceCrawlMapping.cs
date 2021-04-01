using System.ComponentModel.DataAnnotations.Schema;

namespace AMMCrawler.Entities
{
    public class ResourceCrawlMapping
    {
        [ForeignKey(nameof(CrawledLink))]
        public int CrawledLinkID { get; set; }

        [ForeignKey(nameof(FoundLink))]
        public int FoundLinkID { get; set; }

        public virtual ResourceLink CrawledLink { get; set; }
        public virtual ResourceLink FoundLink { get; set; }
    }
}
