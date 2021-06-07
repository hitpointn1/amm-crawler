using AMMCrawler.ServiceLayer.DTO;
using System.Threading.Tasks;

namespace AMMCrawler.ServiceLayer.Abstractions
{
    public interface IGraphService
    {
        Task<GraphDataDto> GetData(string applicationName);
        Task<EdgesCompleteDataDto[]> GetEdgesData(string applicationName);
    }
}
