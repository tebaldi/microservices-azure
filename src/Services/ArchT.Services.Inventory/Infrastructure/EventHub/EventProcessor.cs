using ArchT.Services.Inventory.Infrastructure.Configuration;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.EventHub
{
    class EventProcessor : IEventProcessor
    {
        private readonly ILogger logger;
        private Func<PartitionContext, Task> openAsync;
        private Func<PartitionContext, CloseReason, Task> closeAsync;
        private Func<PartitionContext, Exception, Task> processErrorAsync;
        private Func<PartitionContext, IEnumerable<EventData>, Task> processEventsAsync;

        private EventProcessor(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<EventProcessor>();
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            logger.LogDebug(nameof(IEventProcessor.OpenAsync), context);
            return openAsync?.Invoke(context) ?? Task.CompletedTask;
        }

        Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            logger.LogDebug(nameof(IEventProcessor.CloseAsync), context, reason);
            return closeAsync?.Invoke(context, reason) ?? Task.CompletedTask;
        }

        Task IEventProcessor.ProcessErrorAsync(PartitionContext context, Exception error)
        {
            logger.LogError(nameof(IEventProcessor.ProcessErrorAsync), context, error);
            return processErrorAsync?.Invoke(context, error) ?? Task.CompletedTask;
        }

        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            logger.LogDebug(nameof(IEventProcessor.ProcessEventsAsync), context, messages);
            await processEventsAsync?.Invoke(context, messages);
            await context.CheckpointAsync();
        }

        public class Builder : IEventProcessorFactory
        {
            readonly EventProcessor processor;
            public Builder(ILoggerFactory loggerFactory) => processor = new EventProcessor(loggerFactory);
            public Builder OpenAsync(Func<PartitionContext, Task> func) { processor.openAsync = func; return this; }
            public Builder CloseAsync(Func<PartitionContext, CloseReason, Task> func) { processor.closeAsync = func; return this; }
            public Builder ProcessErrorAsync(Func<PartitionContext, Exception, Task> func) { processor.processErrorAsync = func; return this; }
            public Builder ProcessEventsAsync(Func<PartitionContext, IEnumerable<EventData>, Task> func) { processor.processEventsAsync = func; return this; }
            public EventProcessor Build() => processor;
            IEventProcessor IEventProcessorFactory.CreateEventProcessor(PartitionContext context) => processor;
        }
    }
}
