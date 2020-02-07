using ArchT.Services.Search.Infrastructure.Configuration;
using ArchT.Services.Search.Infrastructure.Extensions;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.EventHub
{
    class EventConsumer
    {
        private readonly EventProcessorHost eventProcessorHost;
        private readonly IEventProcessorFactory factory;

        public EventConsumer(IConfiguration configuration, EventTopics topic, IEventProcessorFactory factory)
        {
            this.factory = factory;

            var config = new EventHubConfig();
            configuration.Bind(nameof(EventHubConfig), config);

            try
            {
                var storage = "";
                switch(topic)
                {
                    case EventTopics.Products:
                        storage = "search-inventory-products-consumer";
                        break;
                    case EventTopics.Orders:
                        storage = "search-trade-orders-consumer";
                        break;
                }

                eventProcessorHost = new EventProcessorHost(
                    topic.Map(),
                    PartitionReceiver.DefaultConsumerGroupName,
                    config.EventHubConnectionString,
                    config.StorageConnectionString,
                    storage);
            }
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); throw ex;  }
        }

        public async Task StartAsync()
        {
            await eventProcessorHost?.RegisterEventProcessorFactoryAsync(factory);
        }
        public async Task StopAsync()
        {
            await eventProcessorHost?.UnregisterEventProcessorAsync();
        }
    }
}
