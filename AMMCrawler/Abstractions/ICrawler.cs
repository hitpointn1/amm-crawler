using AMMCrawler.ServiceLayer.DTO;
using System;
using System.Threading.Tasks;

namespace AMMCrawler.Abstractions
{
    internal interface ICrawler : IDisposable
    {
        Task Crawl(ResourceLinkDto resourceLinkDto);
        Task<ResourceLinkDto> GetAvailableLink(int applicationID);
    }
}
