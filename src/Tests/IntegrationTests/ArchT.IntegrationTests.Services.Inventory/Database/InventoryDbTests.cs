using ArchT.Services.Inventory.Infrastructure.Database;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ArchT.IntegrationTests.Services.Inventory.Database
{
    public class InventoryDbTests : BaseIntegrationTest
    {
        [Fact]
        public void ShouldCreateContext()
        {
            using (var dbContext = new InventoryDb(Configuration))
                Assert.NotNull(dbContext);
        }

        [Fact]
        public async Task ShouldCreateDocument()
        {
            using (var dbContext = new InventoryDb(Configuration))
            {
                var result = await dbContext.CreateItemAsync(
                    new ProductDocument { Id = "pt1", Name = "Teste Product1" });

                try
                {
                    Assert.NotNull(result);
                    Assert.Equal("pt1", result.Id);
                    Assert.Equal("pt1", result.GetPropertyValue<string>(nameof(ProductDocument.Id)));
                    Assert.Equal("Teste Product1", result.GetPropertyValue<string>(nameof(ProductDocument.Name)));
                }
                finally
                {
                    await dbContext.DeleteItemAsync<ProductDocument>(result.Id);
                }
            }
        }
    }
}
