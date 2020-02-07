using ArchT.Services.Inventory.Application.Gateways;
using ArchT.Services.Inventory.Application.Models;
using ArchT.Services.Inventory.Contracts;
using ArchT.Services.Inventory.Infrastructure.Database;
using ArchT.Services.Inventory.Infrastructure.EventHub;
using ArchT.Services.Inventory.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.DataAccess
{
    class ProductRepository : IProductRepository
    {
        private readonly InventoryDb db;
        private readonly EventPublisher publisher;

        public ProductRepository(InventoryDb db, EventPublisher publisher) { this.db = db; this.publisher = publisher; }

        async Task<ProductLoaded> IProductRepository.Load(string productId)
        {
            var document = await db.FindItemAsync<ProductDocument>(productId);
            switch (document)
            {
                case null:
                    return ProductLoaded.Create(productId, string.Empty, 0);

                default:
                    return ProductLoaded.Create(document.Id, document.Name, document.Stock);
            }
        }

        async Task IProductRepository.Store(string productId, params Event[] events)
        {
            var document = await db.FindItemAsync<ProductDocument>(productId);
            switch (document)
            {
                case null:
                    document = new ProductDocument { Id = productId };
                    document.MapEvents(events);
                    await db.CreateItemAsync(document);
                    break;

                default:
                    document.MapEvents(events);
                    await db.UpdateItemAsync(document.Id, document);
                    break;
            }
            var eventContract = document.MapProductUpdated();
            await publisher.PublishAsync(EventTopics.Products, eventContract, document.Id);
        }
    }
}
