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
    class OrdersProcessorFactory : IEventProcessorFactory
    {
        public static EventConsumer CreateConsumer(IConfiguration configuration, ILoggerFactory loggerFactory, SearchDb db) =>
           new EventConsumer(configuration, EventTopics.Orders, new OrdersProcessorFactory(configuration, loggerFactory, db));

        private readonly IConfiguration configuration;
        private readonly ILoggerFactory loggerFactory;
        private readonly SearchDb db;
        public OrdersProcessorFactory(IConfiguration configuration, ILoggerFactory loggerFactory, SearchDb db)
        { this.configuration = configuration; this.loggerFactory = loggerFactory; this.db = db; }

        IEventProcessor IEventProcessorFactory.CreateEventProcessor(PartitionContext context)
        {
            return new EventProcessor.Builder(loggerFactory)
                .ProcessEventsAsync(async (c, events) =>
                {
                    foreach (var e in events)
                    {
                        dynamic data = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Body));
                        string name = data.EventName.ToString();

                        if (name.Contains("OrderUpdated"))
                        {
                            var added = new List<OrderReportData>();
                            var total = 0M;
                            foreach (var l in data.Lines)
                            {
                                total += (int)l.Quantity * (decimal)l.Price;
                            }
                            foreach (var line in data.Lines)
                            {
                                int orderLines = (data.Lines as IEnumerable<dynamic>).Count();
                                string status = "";
                                if(!string.IsNullOrEmpty(data.Processed.ToString()))
                                {
                                    DateTime processedTime = data.Processed.Fields[0];
                                    status = $"Processed on {processedTime}";
                                }
                                DateTime date = data.EventTime;
                                added.Add(new OrderReportData
                                {
                                    OrderId = data.Id.ToString(),
                                    ProductId = line.ProductId.ToString(),
                                    OrderDate = date,
                                    OrderLines = orderLines,
                                    OrderStatus = status,
                                    OrderTotal = total,
                                    Amount = line.Quantity,
                                    Price = line.Price
                                });
                            }
                            string oid = data.Id.ToString();
                            var removedItens = db.OrdersReport.Where(d => d.OrderId == oid).ToArray();
                            db.OrdersReport.RemoveRange(removedItens);
                            db.OrdersReport.AddRange(added);
                            await db.SaveChangesAsync();
                        }                            
                    }

                }).Build();
        }
    }
}
