using ArchT.Services.Inventory.Contracts;
using ArchT.Services.Inventory.Infrastructure.DataAccess;
using ArchT.Services.Inventory.Infrastructure.Database;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ArchT.IntegrationTests.Services.Inventory.DataAccess
{
    public class FindProductTests : BaseIntegrationTest
    {
        readonly FindProduct adapter;

        public FindProductTests()
        {
            adapter = new FindProduct(new InventoryDb(Configuration));
        }

        [Fact]
        public async Task ShouldFindProductByProductId()
        {
            var result = await adapter.Handle(new FindProductRequest { ProductId = "p1" }, CancellationToken.None);
            Assert.NotNull(result);
            Assert.True(result.Completed);
        }
    }
}
