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
    public class UpdateProductNameTest
    {
        readonly Mock<IProductRepository> repository = new Mock<IProductRepository>(); 

        [Fact]
        public async Task ShouldUpdateProductName()
        {
            repository
                .Setup(r => r.Load("p1"))
                .Returns(Task.FromResult(ProductLoaded.Create("p1", string.Empty, 0)));

            var result = await new UpdateProductName(repository.Object).Handle(
                new UpdateProductNameRequest { ProductId = "p1", Name = "name" }, CancellationToken.None);

            Assert.True(result.Completed);
            switch(result.Data)
            {
                case ProductNameUpdated r:
                    Assert.Equal("p1", r.ProductId);
                    Assert.Equal(string.Empty, r.OldValue);
                    Assert.Equal("Name", r.NewValue);
                    break;

                default:
                    Assert.True(false, $"wrong data type {result.Data}");
                    break;
            }
        }
    }
}
