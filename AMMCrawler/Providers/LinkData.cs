namespace AMMCrawler.Providers
{
    public class LinkData
    {
        public string Href { get; set; }
        public string OnClick { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LinkData data &&
                   Href == data.Href;
        }

        public override int GetHashCode()
        {
            return Href.GetHashCode();
        }
    }
}
