using ArchT.Services.Inventory.Application.Gateways;
using ArchT.Services.Inventory.Application.Models;
using ArchT.Services.Inventory.Contracts;
using ArchT.Services.Inventory.Infrastructure.DataAccess;
using ArchT.Services.Inventory.Infrastructure.Database;
using ArchT.Services.Inventory.Infrastructure.EventHub;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ArchT.IntegrationTests.Services.Inventory.DataAccess
{
    public class ProductRepositoryTests : BaseIntegrationTest
    {
        readonly IProductRepository repository;

        public ProductRepositoryTests()
        {
            repository = new ProductRepository(
                new InventoryDb(Configuration), new EventPublisher(Configuration, LoggerFactory));
        }

        [Fact]
        public async Task ShouldStoreAndLoadProduct()
        {
            var events = new Event[] {
                ProductNameUpdated.Create("pt1", string.Empty, "Product Test1"),
                ProductStockIncreased.Create("pt1", 0, 2),
                ProductStockDecreased.Create("pt1", 2, 0),
                ProductStockIncreased.Create("pt1", 0, 1),
            };
            await repository.Store("pt1", events);

            var snapshot = await repository.Load("pt1");
            Assert.NotNull(snapshot);
            Assert.Equal("pt1", snapshot.ProductId);
            Assert.Equal(1, snapshot.Stock);
            Assert.Equal("Product Test1", snapshot.Name);
        }
    }
}
