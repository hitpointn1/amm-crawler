using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMMCrawler.API.Controllers
{
    [Route("/api/{controller}/{action}")]
    public class GraphController : Controller
    {
        private readonly IApplicationService applicationService;
        private readonly IGraphService graphService;

        public GraphController(IApplicationService applicationService, IGraphService graphService)
        {
            this.applicationService = applicationService;
            this.graphService = graphService;
        }

        [HttpGet]
        public Task<string[]> GetApplications()
        {
            return applicationService.GetAll();
        }

        [HttpGet]
        public Task<GraphDataDto> GetData(string applicationName)
        {
            return graphService.GetData(applicationName);
        }
    }
}
