using ArchT.Services.Inventory.Application.Gateways;
using ArchT.Services.Inventory.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Tests.Services.Inventory.Infrastructure.DataAccess
{
    class ProductRepository : IProductRepository
    {
        Task<ProductLoaded> IProductRepository.Load(string productId)
        {
            return Task.FromResult(ProductLoaded.Create(productId, $"Product {productId}", 10));
        }

        Task IProductRepository.Store(string productId, params Event[] events)
        {
            return Task.FromResult(true);
        }
    }
}
