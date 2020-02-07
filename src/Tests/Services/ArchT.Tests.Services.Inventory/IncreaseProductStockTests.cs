using ArchT.Services.Inventory.Application.Gateways;
using ArchT.Services.Inventory.Application.Models;
using ArchT.Services.Inventory.Application.UseCases;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using ArchT.Services.Inventory.Contracts;
using System.Threading;

namespace ArchT.Tests.Services.Inventory
{
    public class IncreaseProductStockTests
    {
        readonly Mock<IProductRepository> repository = new Mock<IProductRepository>(); 

        [Fact]
        public async Task ShouldIncreaseStock()
        {
            repository
                .Setup(r => r.Load("p1"))
                .Returns(Task.FromResult(ProductLoaded.Create("p1", "name", 0)));

            var result = await new IncreaseProductStock(repository.Object).Handle(
                new IncreaseProductStockRequest { ProductId = "p1", Amount = 1 }, CancellationToken.None);

            Assert.True(result.Completed);
            switch(result.Data)
            {
                case ProductStockIncreased r:
                    Assert.Equal("p1", r.ProductId);
                    Assert.Equal(0, r.OldValue);
                    Assert.Equal(1, r.NewValue);
                    break;

                default:
                    Assert.True(false, $"wrong data type {result.Data}");
                    break;
            }
        }
    }
}
