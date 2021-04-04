using AMMCrawler.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMCrawler.DAL.Entities
{
    public class ResourceLink
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public bool IsCrawled { get; set; }
        public LinkType Type { get; set; }


        [ForeignKey(nameof(Application))]
        public int ApplicationID { get; set; }
        public Application Application { get; set; }

        public virtual ICollection<ResourceCrawlMapping> Crawls { get; set; }
    }
}
