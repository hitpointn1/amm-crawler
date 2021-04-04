using AMMCrawler.Enums;

namespace AMMCrawler.Entities
{
    public class ETCLinkMetadata
    {
        public int ID { get; set; }
        public bool IsInternal { get; set; }
        public ETCType Type { get; set; }
    }
}
