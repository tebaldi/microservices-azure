using ArchT.Services.Inventory.Contracts;
using ArchT.Services.Inventory.Infrastructure.EventHub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArchT.IntegrationTests.Services.Inventory.EventHub
{
    public class EventPublisherTest : BaseIntegrationTest
    {
        //[Fact]
        public async Task ShouldPublish()
        {
            var publisher = new EventPublisher(Configuration, LoggerFactory);
            Assert.NotNull(publisher);
            await publisher.PublishAsync(EventTopics.Products, new ProductUpdated
            {
                ProductId = "EventPublisherTest",
                Name = "EventPublisherTest Product",
                Stock = 1
            }, "EventPublisherTest");
        }
    }
}
