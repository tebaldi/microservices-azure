using ArchT.Services.OrderProcessor.Infrastructure.Configuration;
using ArchT.Services.OrderProcessor.Infrastructure.Extensions;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.OrderProcessor.Infrastructure.EventHub
{
    class EventConsumer
    {
        private readonly EventProcessorHost eventProcessorHost;
        private readonly IEventProcessorFactory factory;

        public EventConsumer(IConfiguration configuration, string topic, IEventProcessorFactory factory)
        {
            this.factory = factory;

            var config = new EventHubConfig();
            configuration.Bind(nameof(EventHubConfig), config);

            eventProcessorHost = new EventProcessorHost(
                topic,
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
