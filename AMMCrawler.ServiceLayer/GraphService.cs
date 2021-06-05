using AMMCrawler.Core.Enums;
using AMMCrawler.Core.Helpers;
using AMMCrawler.DAL;
using AMMCrawler.DAL.Entities;
using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMMCrawler.ServiceLayer
{
    public class GraphService : IGraphService
    {
        private readonly CrawlerContext _context;
        public GraphService(CrawlerContext context)
        {
            _context = context;
        }

        public async Task<GraphDataDto> GetData(string applicationName)
        {
            var graphDataDto = new GraphDataDto();
            Application application = await _context.Applications.FirstOrDefaultAsync(a => a.Name == applicationName);

            Task<NodeDto[]> nodesTask = _context.ResourceLinks
                .Where(r => r.ApplicationID == application.ID && r.Type == LinkType.Inner)
                .Select(r => new NodeDto()
                {
                    Caption = r.URL,
                    Id = r.ID,
                    Group = r.Crawls.Count
                })
                .ToArrayAsync();

            Task<EdgeDto[]> edgesTask = _context.ResourceMappings
                .Where(m => m.CrawledLink.ApplicationID == application.ID)
                .Select(r => new EdgeDto()
                {
                    Source = r.CrawledLinkID,
                    Target = r.FoundLinkID,
                    Value = r.CrawledLink.Crawls.Count,
                })
                .OrderBy(e => e.Source)
                .ToArrayAsync();


            NodeDto[] nodes = await nodesTask;
            string rootUrl = ETCLinksAnalyzer.Instance.GetNoProtocolUrl(application.RootURL);
            int i = 0;
            string buffer = "";
            graphDataDto.Nodes = nodes
                .Select(n =>
                {
                    n.Caption = ETCLinksAnalyzer.Instance
                        .GetNoProtocolUrl(n.Caption)
                        .Remove(rootUrl);
                    n.GroupCaption = n.Caption.ReturnBeforeFirstSlash();
                    return n;
                })
                .OrderBy(n => n.GroupCaption)
                .Select(n =>
                {
                    if (n.GroupCaption != buffer)
                    {
                        i++;
                        buffer = n.GroupCaption;
                    }
                    n.Group = i;
                    return n;
                })
                .ToArray();
            graphDataDto.Edges = await edgesTask;

            return graphDataDto;
        }
    }
}
