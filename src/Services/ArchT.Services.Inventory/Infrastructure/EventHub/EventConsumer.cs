using ArchT.Services.Inventory.Infrastructure.Configuration;
using ArchT.Services.Inventory.Infrastructure.Extensions;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.EventHub
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

            eventProcessorHost = new EventProcessorHost(
                topic.Map(),
                PartitionReceiver.DefaultConsumerGroupName,
                config.EventHubConnectionString,
                config.StorageConnectionString,
                config.StorageContainerName);
        }

        public async Task StartAsync()
        {
            await eventProcessorHost.RegisterEventProcessorFactoryAsync(factory);
        }
        public async Task StopAsync()
        {
            await eventProcessorHost?.UnregisterEventProcessorAsync();
        }
    }
}
