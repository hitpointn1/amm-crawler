using AMMCrawler.DTO;
using AMMCrawler.ServiceLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMMCrawler.ServiceLayer.Abstractions
{
    public interface ILinksService
    {
        Task<ResourceLinkDto> GetAvailableLink();
        Task<int> SaveInnerLinks(ResourceLinkDto crawledLink, HashSet<LinkDataDto> innerLinks);
        Task<int> SaveOuterLinks(ResourceLinkDto crawledLink, HashSet<LinkDataDto> outerLinks);
        Task<int> SaveEtcLinks(ResourceLinkDto crawledLink, HashSet<LinkDataDto> etcLinks);
        Task SetResourceLinkAsCrawled(ResourceLinkDto dto);
    }
}
