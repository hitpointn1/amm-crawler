using AMMCrawler.Core.Enums;
using AMMCrawler.DAL;
using AMMCrawler.DAL.Entities;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AMMCrawler.ServiceLayer
{
    public class ApplicationService : IApplicationService
    {
        private readonly CrawlerContext _context;
        public ApplicationService(CrawlerContext context)
        {
            _context = context;
        }

        public async Task<ApplicationRunDto> StartCrawl(string applicationName, string applicationUrl)
        {
            var applicationRunDto = new ApplicationRunDto();
            Application application = await _context.Applications.FirstOrDefaultAsync(a => a.Name == applicationName);
            if (application is null)
            {
                application = new Application()
                {
                    Name = applicationName,
                    RootURL = applicationUrl
                };
                await _context.Applications.AddAsync(application);
                await _context.SaveChangesAsync();
            }
            applicationRunDto.ApplicationID = application.ID;

            var startInfo = new CrawlRunInfo()
            {
                ApplicationID = application.ID,
                Start = DateTime.Now
            };
            await _context.RunInfo.AddAsync(startInfo);
            await _context.SaveChangesAsync();
            applicationRunDto.RunID = startInfo.ID;
            return applicationRunDto;
        }

        public async Task EndCrawl(int crawlRunID)
        {
            CrawlRunInfo runInfo = await _context.RunInfo.FirstOrDefaultAsync(runInfo => runInfo.ID == crawlRunID);
            runInfo.End = DateTime.Now;

            Task<int> innerLinksCountTask = _context.ResourceLinks.Where(r => r.Type == LinkType.Inner).CountAsync();
            Task<int> etcLinksCountTask = _context.ResourceLinks.Where(r => r.Type == LinkType.Etc).CountAsync();
            Task<int> outerLinksCountTask = _context.ResourceLinks.Where(r => r.Type == LinkType.Outer).CountAsync();

            await Task.WhenAll(innerLinksCountTask, etcLinksCountTask, outerLinksCountTask);

            runInfo.InnerCount = innerLinksCountTask.GetAwaiter().GetResult();
            runInfo.EtcCount = etcLinksCountTask.GetAwaiter().GetResult();
            runInfo.OuterCount = outerLinksCountTask.GetAwaiter().GetResult();

            await _context.SaveChangesAsync();
        }
    }
}
