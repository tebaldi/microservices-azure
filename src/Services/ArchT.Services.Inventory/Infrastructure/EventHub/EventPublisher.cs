using ArchT.Services.Inventory.Infrastructure.Configuration;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchT.Services.Inventory.Infrastructure.Extensions;
using ArchT.Services.Inventory.Contracts;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace ArchT.Services.Inventory.Infrastructure.EventHub
{
    class EventPublisher
    {
        private readonly EventHubConfig config = new EventHubConfig();
        private readonly ILogger logger;

        public EventPublisher(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<EventPublisher>();
            configuration.Bind(nameof(EventHubConfig), config);
        }

        public async Task PublishAsync(EventTopics topic, EventContract eventContract, string partitionKey)
        {
            logger.LogDebug(nameof(PublishAsync), topic, eventContract);
            var client = CreateClient(topic);
            try
            {
                var json = JsonConvert.SerializeObject(eventContract);
                var data = new EventData(Encoding.UTF8.GetBytes(json));
                await client.SendAsync(data, partitionKey);
            }
            finally { client?.Close(); }
        }

        private EventHubClient CreateClient(EventTopics topic) =>
            EventHubClient.CreateFromConnectionString(new EventHubsConnectionStringBuilder(
                config.EventHubConnectionString)
            { EntityPath = topic.Map() }.ToString());
    }
}
