using ArchT.Services.Inventory.Contracts;
using ArchT.Services.Inventory.Infrastructure.DataAccess;
using ArchT.Services.Inventory.Infrastructure.Database;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ArchT.IntegrationTests.Services.Inventory.DataAccess
{
    public class GetProductsTests : BaseIntegrationTest
    {
        readonly GetProducts adapter;

        public GetProductsTests()
        {
            adapter = new GetProducts(new InventoryDb(Configuration));
        }

        [Fact]
        public async Task ShouldGetProducts()
        {
            var result = await adapter.Handle(new GetProductsRequest { }, CancellationToken.None);
            Assert.NotNull(result);
            Assert.True(result.Completed);
            Assert.NotNull(result.Data);
        }
    }
}
