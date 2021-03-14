using System.ComponentModel.DataAnnotations.Schema;

namespace AMMCrawler.Entities
{
    public class ResourceCrawlMapping
    {
        public int ID { get; set; }

        [ForeignKey(nameof(CrawledLinks))]
        public int CrawledLinkID { get; set; }

        [ForeignKey(nameof(FoundLinks))]
        public int FoundLinkID { get; set; }

        public virtual ResourceLink CrawledLinks { get; set; }
        public virtual ResourceLink FoundLinks { get; set; }
    }
}
