using AMMCrawler.Configuration;
using AMMCrawler.ServiceLayer.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace AMMCrawler.Export
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder()
               .ConfigureServices((context, services) =>
               {
                   services.ConfigureServices();
                   services.AddTransient<ExcelExporter>();
               })
               .Build();

            IApplicationService appService = host.Services.GetRequiredService<IApplicationService>();

            string[] apps = await appService.GetAll();

            foreach (string app in apps)
                await host.Services.GetRequiredService<ExcelExporter>()
                    .ExportGraphsWithRemovalChance(app);
        }
    }
}
