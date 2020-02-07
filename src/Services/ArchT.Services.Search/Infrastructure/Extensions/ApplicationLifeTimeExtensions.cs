using ArchT.Services.Search.Infrastructure.DataAccess;
using ArchT.Services.Search.Infrastructure.Database;
using ArchT.Services.Search.Infrastructure.EventHub;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.EventHubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.Extensions
{
    static class ApplicationLifeTimeExtensions
    {
        public static void RegisterInfrastructure(this IApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;
            var logger = serviceProvider.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            var consumers = new[]
            {
                OrdersProcessorFactory.CreateConsumer(configuration, logger, new SearchDb(new DbContextOptionsBuilder().Configure(configuration).Options)),
                ProductsProcessorFactory.CreateConsumer(configuration, logger, new SearchDb(new DbContextOptionsBuilder().Configure(configuration).Options))
            };

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                foreach(var consumer in consumers)
                    consumer.StartAsync().Wait();
            });

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                foreach (var consumer in consumers)
                    consumer.StopAsync().Wait();
            });
        }
    }
}
