namespace AMMCrawler.ServiceLayer.DTO
{
    public class LinkDataDto
    {
        public string Href { get; set; }
        public string OnClick { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LinkDataDto data &&
                   Href == data.Href;
        }

        public override int GetHashCode()
        {
            return Href.GetHashCode();
        }
    }
}
