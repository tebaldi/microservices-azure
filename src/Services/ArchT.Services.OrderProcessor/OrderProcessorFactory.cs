using ArchT.Services.OrderProcessor.Infrastructure.Database;
using ArchT.Services.OrderProcessor.Infrastructure.EventHub;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArchT.Services.OrderProcessor
{
    class OrderProcessorFactory : IEventProcessorFactory
    {
        public static EventConsumer CreateConsumer(IConfiguration configuration, ILoggerFactory loggerFactory, OrderProcessorDb db, EventPublisher publisher) =>
            new EventConsumer(configuration, "orders-processing", new OrderProcessorFactory(configuration, loggerFactory, db, publisher));

        private readonly IConfiguration configuration;
        private readonly ILoggerFactory loggerFactory;
        private readonly OrderProcessorDb db;
        private readonly EventPublisher publisher;
        public OrderProcessorFactory(IConfiguration configuration, ILoggerFactory loggerFactory, OrderProcessorDb db, EventPublisher publisher)
        { this.configuration = configuration; this.loggerFactory = loggerFactory; this.db = db; this.publisher = publisher; }
        
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
                            var processedLines = new List<dynamic>();
                            var failedLines = new List<dynamic>();
                            foreach (var line in data.Lines)
                            {
                                using (var httpClient = new HttpClient())
                                {
                                    var inventoryService = configuration.GetSection("ServicesConfig").GetValue<string>("InventoryService");
                                    var uri = $"{inventoryService}/api/v1/inventory/products/{line.ProductId}/decrease-stock";
                                    var postJson = JsonConvert.SerializeObject(new
                                    {
                                        ProductId = line.ProductId,
                                        Amount = line.Quantity
                                    });
                                    var postData = new StringContent(postJson, Encoding.UTF8, "application/json");
                                    var response = await httpClient.PostAsync(uri, postData);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var jsonContent = await response.Content.ReadAsStringAsync();
                                        dynamic responseData = JsonConvert.DeserializeObject(jsonContent);
                                        bool completed = responseData.completed;
                                        if (completed)
                                            processedLines.Add(new { Line = line, Response = responseData });
                                        else
                                            failedLines.Add(new { Line = line, Response = responseData });
                                    }
                                    else
                                        failedLines.Add(new { Line = line, Response = $"{uri}\n{postJson}" });
                                }
                            }

                            if(processedLines.Any())
                                await StoreProcessedOrderAsync(data, processedLines);

                            if(failedLines.Any())
                                await StoreFailedOrderAsync(data, failedLines);
                        }
                    }

                }).Build();
        }

        private async Task StoreProcessedOrderAsync(dynamic data, IEnumerable<dynamic> events)
        {
            string id = data.Id.ToString();
            var document = await db.FindItemAsync<OrderProcessedDocument>(id);
            if (document == null)
            {
                document = new OrderProcessedDocument { Id = id };
                document.Data = data;
                foreach (var evt in events)
                    document.Events.Add(evt);
                await db.CreateItemAsync(document);
            }
            else
            {
                document.Data = data;
                foreach (var evt in events)
                    document.Events.Add(evt);
                await db.UpdateItemAsync(id, document);
            }
        }

        private async Task StoreFailedOrderAsync(dynamic data, IEnumerable<dynamic> events)
        {
            string id = data.Id.ToString();
            var document = await db.FindItemAsync<OrderFailedDocument>(id);
            if (document == null)
            {
                document = new OrderFailedDocument { Id = id };
                document.Data = data;
                foreach (var evt in events)
                    document.Events.Add(evt);
                await db.CreateItemAsync(document);
            }
            else
            {
                document.Data = data;
                foreach (var evt in events)
                    document.Events.Add(evt);
                await db.UpdateItemAsync(id, document);
            }
        }
    }
}
