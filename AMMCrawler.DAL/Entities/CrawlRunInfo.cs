using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMCrawler.DAL.Entities
{
    public class CrawlRunInfo
    {
        public int ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int InnerCount { get; set; }
        public int OuterCount { get; set; }
        public int EtcCount { get; set; }

        [ForeignKey(nameof(Application))]
        public int ApplicationID { get; set; }
        public Application Application { get; set; }
    }
}
