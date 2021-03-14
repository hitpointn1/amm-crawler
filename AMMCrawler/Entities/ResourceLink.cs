using AMMCrawler.Enums;
using System.Collections.Generic;

namespace AMMCrawler.Entities
{
    public class ResourceLink
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public bool IsCrawled { get; set; }
        public LinkType Type { get; set; }

        public virtual ICollection<ResourceCrawlMapping> Crawls { get; set; }
    }
}
