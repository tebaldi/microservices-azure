using ArchT.Services.Search.Contracts;
using ArchT.Services.Search.Infrastructure.Database;
using ArchT.Services.Search.Infrastructure.EventHub;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.DataAccess
{
    class ProductsProcessorFactory : IEventProcessorFactory
    {
        public static EventConsumer CreateConsumer(IConfiguration configuration, ILoggerFactory loggerFactory, SearchDb db) =>
           new EventConsumer(configuration, EventTopics.Products, new ProductsProcessorFactory(configuration, loggerFactory, db));

        private readonly IConfiguration configuration;
        private readonly ILoggerFactory loggerFactory;
        private readonly SearchDb db;
        public ProductsProcessorFactory(IConfiguration configuration, ILoggerFactory loggerFactory, SearchDb db)
        { this.configuration = configuration; this.loggerFactory = loggerFactory; this.db = db;}

        IEventProcessor IEventProcessorFactory.CreateEventProcessor(PartitionContext context)
        {
            return new EventProcessor.Builder(loggerFactory)
                .ProcessEventsAsync(async (c, events) =>
                {
                    foreach (var e in events)
                    {
                        dynamic data = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Body));
                        string name = data.EventName.ToString();

                        if (name.Contains("ProductUpdated"))
                        {
                            string id = data.ProductId.ToString();
                            var p = db.Products.FirstOrDefault(d => d.ProductId == id);
                            switch (p)
                            {
                                case null:
                                    p = new ProductUpdated { ProductId = id, Name = data.Name };
                                    db.Products.Add(p);
                                    break;
                                default:
                                    p.Name = data.Name;
                                    db.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                    break;
                            }
                            await db.SaveChangesAsync();
                        }
                    }

                }).Build();
        }
    }
}
