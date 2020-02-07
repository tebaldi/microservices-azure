using ArchT.Services.Inventory.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Application.Gateways
{
    public interface IProductRepository
    {
        Task<ProductLoaded> Load(string productId);

        Task Store(string productId, params Event[] events);
    }
}
