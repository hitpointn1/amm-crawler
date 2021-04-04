using AMMCrawler.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMCrawler.DAL.Entities
{
    public class ETCLinkMetadata
    {
        public int ID { get; set; }
        public bool IsInternal { get; set; }
        public ETCType Type { get; set; }

        [ForeignKey(nameof(ResourceLink))]
        public int ResourceLinkID { get; set; }
        public ResourceLink ResourceLink { get; set; }
    }
}
