using AMMCrawler.ServiceLayer.DTO;
using System.Threading.Tasks;

namespace AMMCrawler.ServiceLayer.Abstractions
{
    public interface IApplicationService
    {
        Task<ApplicationRunDto> StartCrawl(string applicationName, string applicationUrl);
        Task EndCrawl(int crawlRunID);
    }
}
