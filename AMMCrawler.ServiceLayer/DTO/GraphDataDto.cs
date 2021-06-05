namespace AMMCrawler.ServiceLayer.DTO
{
    public class GraphDataDto
    {
        public EdgeDto[] Edges { get; set; }
        public NodeDto[] Nodes { get; set; }
        public int Clusters { get; set; }
    }
}
