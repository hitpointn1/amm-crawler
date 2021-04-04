﻿using AMMCrawler.ServiceLayer;
using AMMCrawler.ServiceLayer.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AMMCrawler.Configuration
{
    public static partial class ServicesExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services.ConfigureDataSource()
                .AddTransient<IApplicationService, ApplicationService>()
                .AddTransient<ILinksService, LinksService>();
        }
    }
}
