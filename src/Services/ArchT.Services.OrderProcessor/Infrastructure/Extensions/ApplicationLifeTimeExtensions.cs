using ArchT.Services.OrderProcessor.Infrastructure.Database;
using ArchT.Services.OrderProcessor.Infrastructure.EventHub;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchT.Services.OrderProcessor.Infrastructure.Extensions
{
    static class ApplicationLifeTimeExtensions
    {
        public static void RegisterInfrastructure(this IApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;
            var logger = serviceProvider.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            var db = new OrderProcessorDb(configuration);
            var publisher = new EventPublisher(configuration, logger);
            var consumer = OrderProcessorFactory.CreateConsumer(configuration, logger, db, publisher);

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                consumer.StartAsync().Wait();
            });

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                consumer.StopAsync().Wait();
            });
        }
    }
}
