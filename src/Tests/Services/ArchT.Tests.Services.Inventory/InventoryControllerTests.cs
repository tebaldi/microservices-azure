using ArchT.Services.Inventory.Controllers;
using ArchT.Services.Inventory.Contracts;
using MediatR;
using System;
using Xunit;

namespace ArchT.Tests.Services.Inventory
{
    public class InventoryControllerTests : BaseTest
    {
        private readonly InventoryController controller;

        public InventoryControllerTests()
        {
            this.controller = new InventoryController(GetService<IMediator>());
            Assert.NotNull(controller);
        }

        [Fact]
        public void ShouldGet()
        {
            var response = controller.GetProducts(new GetProductsRequest { });
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.Result.Completed);
            Assert.NotEmpty(response.Result.Data);
        }

        [Fact]
        public void ShouldFind()
        {
            var response = controller.FindProduct(new FindProductRequest { ProductId = "p1" });
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.Result.Completed);
            Assert.NotNull(response.Result.Data);
            Assert.Equal("p1", response.Result.Data.ProductId);
        }

        [Fact]
        public void ShouldUpdateName()
        {
            var response = controller.UpdateProductName(new UpdateProductNameRequest { ProductId = "p1", Name = "name" });
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.Result.Completed);
            Assert.NotNull(response.Result.Data);
        }

        [Fact]
        public void ShouldIncreaseStock()
        {
            var response = controller.IncreaseProductStock(new IncreaseProductStockRequest { ProductId = "p1", Amount = 1 });
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.Result.Completed);
            Assert.NotNull(response.Result.Data);
        }

        [Fact]
        public void ShouldDecreaseStock()
        {
            var response = controller.DecreaseProductStock(new DecreaseProductStockRequest { ProductId = "p1", Amount = 1 });
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.Result.Completed);
            Assert.NotNull(response.Result.Data);
        }
    }
}
